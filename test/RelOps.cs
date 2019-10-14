using System;

class X
{
    static void Test1()
    {
        var A = 100;
        var B = 200;
        Console.WriteLine("--Test1");
        string c = "<";
        if (A < B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test2()
    {
        var A = 100;
        var B = 200;
        Console.WriteLine("--Test2");
        string c = "<=";
        if (A <= B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test3()
    {
        var A = 100;
        var B = 200;
        Console.WriteLine("--Test3");
        string c = ">";
        if (A > B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test4()
    {
        var A = 100;
        var B = 200;
        Console.WriteLine("--Test4");
        string c = ">=";
        if (A >= B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
    }
}