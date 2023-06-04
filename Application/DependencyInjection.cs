using System.Reflection;
using Application.Behaviors;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IInputService, FileLoader>()
            .AddScoped<IOutputService, DatabaseStructureWriter>()
            .AddScoped<IFormatParser, TSVParser>();
        return services;
    }
}