using Application.Commands.Create.JobTitle;
using Application.Interfaces;
using MediatR;

namespace Application.InputHandlers;

public class JobTitleInputHandler : EntityInputHandler
{
    public JobTitleInputHandler(IExceptionHandler errorHandler, IMediator mediator) 
        : base(errorHandler, mediator)
    {
    }

    protected override IRequest CreateEntityCommand(string[] parsedLine)
    {
        return new CreateJobTitleCommand
        {
            Title = parsedLine[0].FixRegistry()
        };
    }
}