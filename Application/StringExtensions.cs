namespace Application;

public static class StringExtensions
{
    public static string FixRegistry(this string s)
    {
        s = s.ToLower();
        if (s.Length > 0)
            s = char.ToUpper(s[0]) + s[1..];
        return s;
    }
    
    public static string FixFullNameRegistry(this string s)
    {
        return string.Join(' ', s.Split(' ').Select(x => x.FixRegistry()));
    }
}