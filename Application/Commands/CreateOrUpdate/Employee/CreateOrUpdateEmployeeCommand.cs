using MediatR;

namespace Application.Commands.CreateOrUpdate.Employee;

public class CreateOrUpdateEmployeeCommand : IRequest
{
    public string DepartmentName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
}