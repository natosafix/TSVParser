﻿namespace Application.Interfaces;

public interface IExceptionHandler
{
    void Handle(Exception exception);
}