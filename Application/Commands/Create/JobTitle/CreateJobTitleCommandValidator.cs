using FluentValidation;

namespace Application.Commands.Create.JobTitle;

public class CreateJobTitleCommandValidator : AbstractValidator<CreateJobTitleCommand>
{
    public CreateJobTitleCommandValidator()
    {
        RuleFor(command => command.Title).NotEmpty();
    }
}