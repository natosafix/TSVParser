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
    .AddScoped<ConsoleUi>()
    .AddScoped<IExceptionHandler, ConsoleExceptionHandler>()
    .AddScoped<IDataWriter, ConsoleWriter>()
    .BuildServiceProvider();

var ui = serviceProvider.GetRequiredService<ConsoleUi>();
await ui.Run();


