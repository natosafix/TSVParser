using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.CreateOrUpdate.Department;

public class CreateOrUpdateDepartmentCommandHandler : IRequestHandler<CreateOrUpdateDepartmentCommand>
{
    private readonly ICompanyDbContext dbContext;

    public CreateOrUpdateDepartmentCommandHandler(ICompanyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(CreateOrUpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        Domain.EntityTypes.Department parentDepartment;
        if (request.ParentName != string.Empty)
        {
            parentDepartment = await dbContext.Departments.FirstOrDefaultAsync(e => e.Name == request.ParentName,
                cancellationToken);
        }
        else
        {
            parentDepartment = await dbContext.Departments.FindAsync(new object[] {0});
        }
        
        Domain.EntityTypes.Employee? manager = null;
        if (request.ManagerFullName != string.Empty)
        {
            manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.FullName == request.ManagerFullName, 
                cancellationToken);
        }
        
        var department =
            await dbContext.Departments.FirstOrDefaultAsync(e =>
                e.ParentId == parentDepartment.Id && e.Name == request.Name, cancellationToken);

        if (department is null)
        {
            await dbContext.Departments.AddAsync(new Domain.EntityTypes.Department
            {
                Parent = parentDepartment,
                Name = request.Name,
                Phone = request.Phone,
                Manager = manager
            }, cancellationToken);
        }
        else
        {
            department.Manager = manager;
            department.Phone = request.Phone;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}