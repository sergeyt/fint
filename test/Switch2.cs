using System;

class Switch2
{
    static void f(int a, int b)
    {
        Console.WriteLine("begin");
        switch (a)
        {
            case 0:
                Console.WriteLine("a == 0");
                if (b > a)
                {
                    Console.WriteLine("a > b");
                    Console.WriteLine("T");
                }
                else
                {
                    Console.WriteLine("a <= b");
                    Console.WriteLine("F");
                }
                break;

            case 1:
                Console.WriteLine("a == 1");
                for (int i = 0; i < 5; ++i)
                {
                    Console.WriteLine("i");
                    Console.WriteLine(i);
                    if (b == i)
                    {
                        Console.WriteLine("b == i");
                        Console.WriteLine("T");
                    }
                    else
                    {
                        Console.WriteLine("b != i");
                        Console.WriteLine("F");
                    }
                }
                return;
        }
        Console.WriteLine("end");
    }

    static void Main()
    {
        for (int a = 0; a <= 5; ++a)
        {
            for (int b = 0; b <= 5; ++b)
            {
                f(a, b);
            }
        }
    }
}