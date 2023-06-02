using Application.Commands;
using Application.Commands.Create.JobTitle;
using Application.Commands.CreateOrUpdate.Employee;
using MediatR;

namespace Application;

public class InputCommand
{
    private readonly TSVParser tsvParser;
    private readonly IMediator mediator;

    public InputCommand(TSVParser tsvParser, IMediator mediator)
    {
        this.tsvParser = tsvParser;
        this.mediator = mediator;
    }
    public async Task Handle(string filepath, int mode)
    {
        using (var streamReader = new StreamReader(filepath))
        {
            string line;
            streamReader.ReadLine();
            while ((line = streamReader.ReadLine()) != null)
            {
                var parsedString = tsvParser.Parse(line).ToArray();
                IRequest command;
                switch (mode)
                {
                    case 1:
                        command = new CreateOrUpdateDepartmentCommand
                        {
                            Name = parsedString[0],
                            ParentName = parsedString[1],
                            ManagerFullName = parsedString[2],
                            Phone = parsedString[3]
                        };
                        break;
                    case 2:
                        command = new CreateOrUpdateEmployeeCommand
                        {
                            DepartmentName = parsedString[0],
                            FullName = parsedString[1],
                            Login = parsedString[2],
                            Password = parsedString[3],
                            JobTitle = parsedString[4]
                        };
                        break;
                    case 3:
                        command = new CreateJobTitleCommand
                        {
                            Title = parsedString[0]
                        };
                        break;
                    default:
                        command = null;
                        break;
                }

                await mediator.Send(command);
            }
        }
    }
}