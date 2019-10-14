using System;

class BitManipulation
{
    static void Main()
    {
        int a = 1;
        int b = 1;
        Console.WriteLine(a << b);
        Console.WriteLine(a >> b);
        Console.WriteLine(a & b);
        Console.WriteLine(a | b);
        Console.WriteLine(a ^ b);
        Console.WriteLine(~a);
    }
}