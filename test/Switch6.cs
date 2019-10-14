using System;

class Switch6
{
    static void f(int n)
    {
        Console.WriteLine("f begin");
        switch (n)
        {
            case 1:
            case 2:
            case 3:
                Console.WriteLine("1 - 3");
                break;

            case 10:
            case 11:
            case 12:
                Console.WriteLine("10 - 12");
                break;

            default:
                Console.WriteLine("default");
                break;
        }
        Console.WriteLine("f end");
    }

    static void Main()
    {
        for (int i = 0; i < 30; i += 3)
            f(i);
    }
}