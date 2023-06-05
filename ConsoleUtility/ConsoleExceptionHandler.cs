using System;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;

namespace ConsoleUtility;

public class ConsoleExceptionHandler : IExceptionHandler
{
    public void Handle(Exception exception)
    {
        Console.Error.WriteLine($"Catched exception:\n {exception.Message}");
        if (exception.InnerException is not null)
            Console.Error.WriteLine($" {exception.InnerException.Message}");
    }
}