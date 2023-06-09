﻿using System.Reflection;
using Application.Behaviors;
using Application.InputHandlers;
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
        services.AddScoped<IFileParser, LazyFileParser>()
            .AddScoped<IOutputService, DatabaseStructureWriter>()
            .AddScoped<IFormatParser, TSVParser>()
            .AddScoped<IInputHandler, InputHandler>()
            .AddScoped<DepartmentInputHandler>()
            .AddScoped<EmployeeInputHandler>()
            .AddScoped<JobTitleInputHandler>();
        return services;
    }
}