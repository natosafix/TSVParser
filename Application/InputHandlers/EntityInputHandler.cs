using Application.Interfaces;
using MediatR;

namespace Application.InputHandlers;

public abstract class EntityInputHandler
{
    private readonly IExceptionHandler errorHandler;
    private readonly IMediator mediator;

    protected EntityInputHandler(IExceptionHandler errorHandler, IMediator mediator)
    {
        this.errorHandler = errorHandler;
        this.mediator = mediator;
    }

    public async Task Handle(List<string[]> parsedLines)
    {
        foreach (var parsedLine in parsedLines)
        {
            var command = CreateEntityCommand(parsedLine);
            try
            {
                await mediator.Send(command);
            }
            catch (Exception e)
            {
                errorHandler.Handle(new ArgumentException($"Broken record {string.Join(' ', parsedLine)}", e));
            }
        }
    }

    protected abstract IRequest CreateEntityCommand(string[] parsedLine);
}