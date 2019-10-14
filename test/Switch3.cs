using System;

class Switch3
{
    static void f(int a, int b)
    {
        Console.WriteLine("begin");
        switch (a)
        {
            case 0:
                Console.WriteLine("case 0");
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
                Console.WriteLine("case 1");
                return;

            case 2:
                Console.WriteLine("case 2");
                goto case 0;

            case 3:
                Console.WriteLine("case 3");
                if (b > 1)
                    goto case 1;
                else
                    goto case 2;

            default:
                Console.WriteLine("case default");
                break;
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