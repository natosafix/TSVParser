using System.ComponentModel.DataAnnotations;
using Application.Commands;
using Application.Commands.Create.JobTitle;
using Application.Commands.CreateOrUpdate.Employee;
using Application.Interfaces;
using MediatR;

namespace Application;

public class FileLoader : IInputService
{
    private readonly IFormatParser tsvParser;
    private readonly IMediator mediator;
    private readonly IExceptionHandler errorHandler;

    public FileLoader(IExceptionHandler errorHandler, IFormatParser tsvParser, IMediator mediator)
    {
        this.tsvParser = tsvParser;
        this.mediator = mediator;
        this.errorHandler = errorHandler;
    }
    public async Task Load(string filepath, DataType dataType)
    {
        using (var streamReader = new StreamReader(filepath))
        {
            string line;
            await streamReader.ReadLineAsync();
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                var parsedString = tsvParser.ParseLine(line).ToArray();
                IRequest command;
                
                switch (dataType)
                {
                    case DataType.Department:
                        command = new CreateOrUpdateDepartmentCommand
                        {
                            Name = parsedString[0].FixRegistry(),
                            ParentName = parsedString[1].FixRegistry(),
                            ManagerFullName = parsedString[2].FixFullNameRegistry(),
                            Phone = parsedString[3]
                        };
                        break;
                    case DataType.Employee:
                        command = new CreateOrUpdateEmployeeCommand
                        {
                            DepartmentName = parsedString[0].FixRegistry(),
                            FullName = parsedString[1].FixFullNameRegistry(),
                            Login = parsedString[2],
                            Password = parsedString[3],
                            JobTitle = parsedString[4].FixRegistry()
                        };
                        break;
                    case DataType.JobTitle:
                        command = new CreateJobTitleCommand
                        {
                            Title = parsedString[0].FixRegistry()
                        };
                        break;
                    default:
                        command = null;
                        break;
                }

                try
                {
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