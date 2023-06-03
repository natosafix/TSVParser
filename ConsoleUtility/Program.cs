using System.ComponentModel.Design;
using Application;
using Application.Interfaces;
using ConsoleUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();

var serviceProvider = new ServiceCollection()
    .AddPersistence(config)
    .AddApplication()
    .AddScoped<InputCommand>()
    .AddScoped<OutputScenario>()
    .AddScoped<IFormatParser, TSVParser>()
    .AddScoped<IExceptionHandler, ConsoleExceptionHandler>()
    .AddScoped<IDataWriter, ConsoleWriter>()
    .BuildServiceProvider();

var dbContext = serviceProvider.GetService<CompanyDbContext>();
var inputCommand = serviceProvider.GetRequiredService<InputCommand>();
/*await inputCommand.Handle("jobtitle.tsv", 3);
await inputCommand.Handle("departments.tsv", 1);
await inputCommand.Handle("employees.tsv", 2);*/
var outputScenario = serviceProvider.GetRequiredService<OutputScenario>();
/*await outputScenario.GetDatabaseStructure();
Console.WriteLine();
await outputScenario.GetDatabaseStructure(5);*/


var helloMessage = @"Hello. It's utility for loading data in tsv format into the database and get information about the database structure.
Use help for more information.";
var helpMessage = @"Available commands:
	input {filename} {datatype}
		where datatype takes one of the values: d (department) / e (employee) / j (job title)
		loading data in tsv format of selected datatype from filename into the database
	output [department id]
		get information about the database structure
		if department id is passed shows department structure
	help 
		show available commands
    exit";
Console.WriteLine(helloMessage);
while (true)
{
    Console.Write("->");
    var message = Console.ReadLine();
    var messageArgs = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (message.StartsWith("input"))
    {
        if (messageArgs.Length != 3)
        {
            Console.WriteLine(helpMessage);
            continue;
        }

        var dataType = 0;
        if (messageArgs[2] == "d")
            dataType = 1;
        if (messageArgs[2] == "e")
            dataType = 2;
        if (messageArgs[2] == "j")
            dataType = 3;
        if (dataType == 0)
        {
            Console.WriteLine(helpMessage);
            continue;
        }
        await inputCommand.Handle(messageArgs[1], dataType);
    }
    else if (message.StartsWith("output"))
    {
        if (messageArgs.Length > 2)
        {
            Console.WriteLine(helpMessage);
            continue;
        }

        if (messageArgs.Length == 2)
        {
            await outputScenario.GetDatabaseStructure(int.Parse(messageArgs[1]));
        }
        else
        {
            await outputScenario.GetDatabaseStructure();
        }
    }
    else if (message.StartsWith("exit"))
    {
        Environment.Exit(0);
    }
    else 
        Console.WriteLine(helpMessage);
}

