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
await inputCommand.Handle("jobtitle.tsv", 3);
await inputCommand.Handle("departments.tsv", 1);
await inputCommand.Handle("employees.tsv", 2);
var outputScenario = serviceProvider.GetRequiredService<OutputScenario>();
await outputScenario.GetDatabaseStructure();
Console.WriteLine();
await outputScenario.GetDatabaseStructure(5);


/*while (true)
{ 
    Console.Error.WriteLine("command type");
    var a = Console.ReadLine();
    if (a == "exit")
        Environment.Exit(0);
    if (a == "import")
    {
        var path = Console.ReadLine();
    }

    if (a == "output")
    {
        var id = Console.ReadLine();
    }
}*/

