namespace Application.Interfaces;

public interface IInputService
{
    Task Load(string filepath, DataType dataType);
}