using Application;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleUtility;

public class ConsoleUi
{
    private readonly IInputService inputService;
    private readonly IOutputService outputService;
    public ConsoleUi(IInputService inputService, IOutputService outputService)
    {
        this.inputService = inputService;
        this.outputService = outputService;
    }

    #region staticMessages
    private const string HelloMessage = @"Hello. It's utility for loading data in tsv format into the database and get information about the database structure.
        Use help for more information.";
    private const string HelpMessage = @"Available commands:
        	input {filename} {dataType}
        		where datatype takes one of the values: d (department) / e (employee) / j (job title)
        		loading data in tsv format of selected datatype from filename into the database
        	output [department id]
        		get information about the database structure
        		if department id is passed shows department structure
        	help 
        		show available commands
            exit";
    #endregion
    
    public async Task Run()
    {
        Console.WriteLine(HelloMessage);
        while (true)
        {
            Console.Write("->");
            var message = Console.ReadLine();
            var messageArgs = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (message.StartsWith("input"))
            {
                await HandleInputCommand(messageArgs);
            }
            else if (message.StartsWith("output"))
            {
                await HandleOutputCommand(messageArgs);
            }
            else if (message.StartsWith("exit"))
                Environment.Exit(0);
            else
                WriteIncorrectFormat($"unknown command");
        }
    }

    private async Task HandleInputCommand(string[] messageArgs)
    {
        if (messageArgs.Length != 3)
        {
            WriteIncorrectFormat($"input command require only 2 parameters, given {messageArgs.Length - 1}");
            return;
        }

        DataType dataType;
        switch (messageArgs[2])
        {
            case "d":
                dataType = DataType.Department;
                break;
            case "e":
                dataType = DataType.Employee;
                break;
            case "j":
                dataType = DataType.JobTitle;
                break;
            default:
                WriteIncorrectFormat($"input command require file dataType, given {messageArgs[2]}");
                return;
        }
                
        await inputService.Load(messageArgs[1], dataType);
    }

    private async Task HandleOutputCommand(string[] messageArgs)
    {
        switch (messageArgs.Length)
        {
            case > 2:
                WriteIncorrectFormat($"output command 0 or 1 parameters, given {messageArgs.Length - 1}");
                return;
            case 2:
                await outputService.WriteDatabaseStructure(int.Parse(messageArgs[1]));
                break;
            default:
                await outputService.WriteDatabaseStructure();
                break;
        }
    }

    private void WriteIncorrectFormat(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine(HelpMessage);
    }
}

