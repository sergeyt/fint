using System;

class Switch5
{
    static bool IsWhiteSpace(char c)
    {
        bool result;
        switch (c)
        {
            case (char)0x9:
            case (char)0x0a:
            case (char)0x0b:
            case (char)0x0c:
            case (char)0x0d:
            case (char)0x85: // NEL 
            case (char)0x2028: // Line Separator
            case (char)0x2029: // Paragraph Separator
                result = true;
                break;

            default:
                result = false;
                break;
        }
        return result;
    }

    static void f(char c)
    {
        Console.WriteLine(IsWhiteSpace(c));
    }

    static void Main()
    {
        f(' ');
        f('a');
    }
}