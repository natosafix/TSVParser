namespace Application;

public class TSVParser
{
    public IEnumerable<string> Parse(string input)
    {
        var tabSplitted = input.Split("\t");
        return tabSplitted.Select(x => x.Trim());
    }
}