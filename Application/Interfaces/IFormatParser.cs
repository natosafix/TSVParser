namespace Application.Interfaces;

public interface IFormatParser
{
    IEnumerable<string> ParseLine(string line);
}