namespace Application.Interfaces;

public interface IFileParser
{
    IAsyncEnumerable<List<string[]>> ParseFile(string filepath);
}