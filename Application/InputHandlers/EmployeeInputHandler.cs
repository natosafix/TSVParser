using Application.Commands.CreateOrUpdate.Employee;
using Application.Interfaces;
using MediatR;

namespace Application.InputHandlers;

public class EmployeeInputHandler : EntityInputHandler
{
    public EmployeeInputHandler(IExceptionHandler errorHandler, IMediator mediator) 
        : base(errorHandler, mediator)
    {
    }

    protected override IRequest CreateEntityCommand(string[] parsedLine)
    {
        return new CreateOrUpdateEmployeeCommand
        {
            DepartmentName = parsedLine[0].FixRegistry(),
            FullName = parsedLine[1].FixFullNameRegistry(),
            Login = parsedLine[2],
            Password = parsedLine[3],
            JobTitle = parsedLine[4].FixRegistry()
        };
    }
}