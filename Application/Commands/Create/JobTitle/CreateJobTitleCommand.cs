using MediatR;

namespace Application.Commands.Create.JobTitle;

public class CreateJobTitleCommand : IRequest
{
    public string Title { get; set; } = null!;
}