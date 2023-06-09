﻿using Application;
using Application.Interfaces;
using ConsoleUtility;
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

AppDomain.CurrentDomain.UnhandledException += ProcessUnhandledException;

var ui = serviceProvider.GetRequiredService<ConsoleUi>();
await ui.Run();

static void ProcessUnhandledException(object sender, UnhandledExceptionEventArgs args)
{
    Console.Error.WriteLine((args.ExceptionObject as Exception)?.Message);
    Console.Error.WriteLine((args.ExceptionObject as Exception)?.StackTrace);
    Environment.Exit(1);
}
