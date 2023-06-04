using Application.InputHandlers;

namespace Application.Interfaces;

public interface IInputHandler
{
    Task Handle(string filepath, EntityInputHandler entityInputHandler);
}