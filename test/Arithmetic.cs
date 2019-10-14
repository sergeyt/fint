using System;

class Arithmetic
{
    private static void Test(int a, int b)
    {
        Console.WriteLine(a + b);
        Console.WriteLine(a - b);
        Console.WriteLine(a * b);
        Console.WriteLine(a / b);
        Console.WriteLine(a % b);
    }

    public static void Main()
    {
        Console.WriteLine("Test1");
        Test(4, 2);
        Console.WriteLine("Test2");
        Test(8, 3);
    }
}