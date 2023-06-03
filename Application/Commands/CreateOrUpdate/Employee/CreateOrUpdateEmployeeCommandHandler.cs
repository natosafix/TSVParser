using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.EntityTypes;

namespace Application.Commands.CreateOrUpdate.Employee;

public class CreateOrUpdateEmployeeCommandHandler : IRequestHandler<CreateOrUpdateEmployeeCommand>
{
    private readonly ICompanyDbContext dbContext;

    public CreateOrUpdateEmployeeCommandHandler(ICompanyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(CreateOrUpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var department =
            await dbContext.Departments.FirstOrDefaultAsync(e => e.Name == request.DepartmentName, cancellationToken);
        if (department is null)
            throw new NotFoundException(nameof(Department), request.DepartmentName);

        var jobTitle =
            await dbContext.JobTitles.FirstOrDefaultAsync(e => e.Title == request.JobTitle, cancellationToken);
        if (jobTitle is null)
            throw new NotFoundException(nameof(JobTitle), request.JobTitle);

        var manager =
            await dbContext.Employees.FirstOrDefaultAsync(e => e.FullName == request.FullName, cancellationToken);

        if (manager is null)
        {
            await dbContext.Employees.AddAsync(new Domain.EntityTypes.Employee
            {
                Department = department,
                FullName = request.FullName,
                Login = request.Login,
                Password = request.Password,
                JobTitle = jobTitle,
                JobTitleId = jobTitle.Id
            }, cancellationToken);
        }
        else
        {
            manager.Department = department;
            manager.Login = request.Login;
            manager.Password = request.Password;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}