using System;

class For1
{
    static void f1()
    {
        for (int i = 0; i < 5; ++i)
            Console.WriteLine(i);
    }

    static void f2()
    {
        Console.WriteLine("For1");
        for (int i = 0; i < 5; ++i)
        {
            Console.WriteLine("i");
            Console.WriteLine(i);
        }
        Console.WriteLine("end");
    }

    static void f3()
    {
        Console.WriteLine("For2");
        for (int i = 0; i < 5; ++i)
        {
            Console.WriteLine(i * i);
        }
    }

    public static void Main()
    {
        f1();
        f2();
        f3();
    }
}