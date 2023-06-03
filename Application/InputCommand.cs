using System.ComponentModel.DataAnnotations;
using Application.Commands;
using Application.Commands.Create.JobTitle;
using Application.Commands.CreateOrUpdate.Employee;
using Application.Interfaces;
using MediatR;

namespace Application;

public class InputCommand
{
    private readonly IFormatParser tsvParser;
    private readonly IMediator mediator;
    private readonly IExceptionHandler errorHandler;

    public InputCommand(IFormatParser tsvParser, IMediator mediator, IExceptionHandler errorHandler)
    {
        this.tsvParser = tsvParser;
        this.mediator = mediator;
        this.errorHandler = errorHandler;
    }
    public async Task Handle(string filepath, int mode)
    {
        using (var streamReader = new StreamReader(filepath))
        {
            string line;
            await streamReader.ReadLineAsync();
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                var parsedString = tsvParser.ParseLine(line).ToArray();
                IRequest command;
                try
                {
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
                catch (Exception e)
                {
                    errorHandler.Handle(e);
                }
            }
        }
    }
}