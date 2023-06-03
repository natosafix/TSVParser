using FluentValidation;

namespace Application.Commands.CreateOrUpdate.Employee;

public class CreateOrUpdateEmployeeValidator : AbstractValidator<CreateOrUpdateEmployeeCommand>
{
    public CreateOrUpdateEmployeeValidator()
    {
        RuleFor(command => command.FullName).NotEmpty();
        RuleFor(command => command.Login).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
        RuleFor(command => command.JobTitle).NotEmpty();
        RuleFor(command => command.DepartmentName).NotEmpty();
    }
}