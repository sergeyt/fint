using System;

class ForTest2
{
    static void f1()
    {
        Console.WriteLine("NestedFors");
        for (int a = 1; a <= 5; ++a)
        {
            for (int b = 1; b <= 5; ++b)
            {
                Console.WriteLine(a * b);
            }
        }
        Console.WriteLine("end");
    }

    static void Main()
    {
        f1();
    }
}