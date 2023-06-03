using FluentValidation;

namespace Application.Commands.CreateOrUpdate.Department;

public class CreateOrUpdateDepartmentValidator : AbstractValidator<CreateOrUpdateDepartmentCommand>
{
    public CreateOrUpdateDepartmentValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Phone).NotEmpty();
    }
}