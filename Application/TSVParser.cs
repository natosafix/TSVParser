using System.Text.RegularExpressions;
using Application.Interfaces;

namespace Application;

public class TSVParser : IFormatParser
{
    public IEnumerable<string> ParseLine(string line)
    {
        var tabSplitted = line.Split("\t", StringSplitOptions.TrimEntries);
        return tabSplitted.Select(x => Regex.Replace(x, @"\s+", " "));
    }
}