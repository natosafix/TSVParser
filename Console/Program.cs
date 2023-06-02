using Application;
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
    .AddScoped<TSVParser>()
    .BuildServiceProvider();

var dbContext = serviceProvider.GetService<CompanyDbContext>();
var inputCommand = serviceProvider.GetRequiredService<InputCommand>();


