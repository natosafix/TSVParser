using Application.Commands.CreateOrUpdate.Department;
using Application.Interfaces;
using MediatR;

namespace Application.InputHandlers;

public class DepartmentInputHandler : EntityInputHandler
{
    public DepartmentInputHandler(IExceptionHandler errorHandler, IMediator mediator) 
        : base(errorHandler, mediator)
    {
    }

    protected override IRequest CreateEntityCommand(string[] parsedLine)
    {
        return new CreateOrUpdateDepartmentCommand
        {
            Name = parsedLine[0].FixRegistry(),
            ParentName = parsedLine[1].FixRegistry(),
            ManagerFullName = parsedLine[2].FixFullNameRegistry(),
            Phone = parsedLine[3]
        };
    }
}