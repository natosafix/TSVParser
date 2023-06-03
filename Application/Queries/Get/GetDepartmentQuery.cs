using Domain.EntityTypes;
using MediatR;

namespace Application.Queries.Get;

public class GetDepartmentQuery : IRequest<Department>
{
    public int DepartmentId { get; set; }
}