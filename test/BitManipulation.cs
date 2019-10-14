using System;

class BitManipulation
{
    static void f1()
    {
        int a = 400;
        int b = 200;
        Console.WriteLine(a << b);
        Console.WriteLine(a >> b);
        Console.WriteLine(a & b);
        Console.WriteLine(a | b);
        Console.WriteLine(a ^ b);
        Console.WriteLine(~a);
    }

    static void f2()
    {
        uint a = 400;
        uint b = 200;
        Console.WriteLine(a << 1);
        Console.WriteLine(a >> 1);
        Console.WriteLine(a & b);
        Console.WriteLine(a | b);
        Console.WriteLine(a ^ b);
        Console.WriteLine(~a);
    }

    static void Main()
    {
        f1();
        f2();
    }
}
