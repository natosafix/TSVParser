using Application.Interfaces;

namespace ConsoleUtility;

public class ConsoleWriter : IDataWriter
{
    public void WriteLine(string str)
    {
        Console.WriteLine(str);
    }
}