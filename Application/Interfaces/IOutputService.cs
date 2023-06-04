namespace Application.Interfaces;

public interface IOutputService
{
    Task WriteDatabaseStructure();
    Task WriteDatabaseStructure(int departmentId);
}