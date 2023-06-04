using MediatR;

namespace Application.Commands.CreateOrUpdate.Department;

public class CreateOrUpdateDepartmentCommand : IRequest
{
    public string Name { get; set; } = null!;
    public string? ParentName { get; set; }
    public string? ManagerFullName { get; set; }
    public string Phone { get; set; } = null!;
}