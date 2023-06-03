using System;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;

namespace ConsoleUtility;

public class ConsoleExceptionHandler : IExceptionHandler
{
    public void Handle(Exception exception)
    {
        Console.Error.WriteLine($"Catched exception: {exception.Message}");
    }
}