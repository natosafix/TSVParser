using Application.Exceptions;
using Application.Interfaces;
using Domain.EntityTypes;
using MediatR;

namespace Application.Queries.Get;

public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, Department>
{
    private readonly ICompanyDbContext dbContext;

    public GetDepartmentQueryHandler(ICompanyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Department> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments.FindAsync(new object[] {request.DepartmentId}, cancellationToken);
        if (department is null)
            throw new NotFoundException(nameof(Department), request.DepartmentId);
        return department;
    }
}