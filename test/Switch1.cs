using System;

class Switch1
{
    static void f1(int n)
    {
        Console.WriteLine("f1");
        switch (n)
        {
            case 0:
                Console.WriteLine("0");
                break;

            case 1:
                Console.WriteLine("1");
                return;

            case 2:
                Console.WriteLine("2");
                break;
        }
        Console.WriteLine("end");
    }

    static void f2(int n)
    {
        Console.WriteLine("f2");
        switch (n)
        {
            case 0:
                Console.WriteLine("0");
                break;

            case 1:
                Console.WriteLine("1");
                return;

            default:
                Console.WriteLine("default");
                break;
        }
        Console.WriteLine("end");
    }

    static void f3(int n)
    {
        Console.WriteLine("f3");
        switch (n)
        {
            case 1:
                Console.WriteLine("1");
                break;

            case 2:
                Console.WriteLine("2");
                return;
        }
        Console.WriteLine("end");
    }

    static void f4(int n)
    {
        Console.WriteLine("f4");
        switch (n)
        {
            case 1:
                Console.WriteLine("1");
                break;

            case 3:
                Console.WriteLine("3");
                return;

            case 5:
                Console.WriteLine("5");
                return;

            default:
                Console.WriteLine("default");
                return;
        }
        Console.WriteLine("end");
    }

    public static void Main()
    {
        f1(0);
        f1(1);
        f1(2);
        f1(4);

        f2(0);
        f2(1);
        f2(2);

        f3(0);
        f3(1);
        f3(2);

        f4(0);
        f4(1);
        f4(2);
        f4(3);
        f4(4);
        f4(5);
        f4(6);
    }
}