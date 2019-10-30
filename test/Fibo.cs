using System;

class Fibo
{
    private static int fib(int x)
    {
        if (x > 1)
        {
            return fib(x - 1) + fib(x - 2);
        }
        return x;
    }

    public static void Main()
    {
        for (int i = 0; i < 10; ++i)
        {
            Console.WriteLine(fib(i));
        }
    }
}
