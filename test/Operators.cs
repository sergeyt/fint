using System;

class Operators
{
    static void f1()
    {
        int a = 400, b = -200;
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);

        Console.WriteLine("Multiplicative operators");
        Console.WriteLine("{0} * {1} = {2}", a, b, a * b);
        Console.WriteLine("{0} / {1} = {2}", a, b, a / b);
        Console.WriteLine("{0} % {1} = {2}", a, b, a % b);

        Console.WriteLine("Additive operators");
        Console.WriteLine("{0} + {1} = {2}", a, b, a + b);
        Console.WriteLine("{0} - {1} = {2}", a, b, a - b);

        Console.WriteLine("Shift operators");
        Console.WriteLine("{0} >> {1} = {2}", a, 8, a >> 8);
        Console.WriteLine("{0} << {1} = {2}", b, 8, b << 8);

        Console.WriteLine("Relational operators");
        Console.WriteLine("{0} < {1} = {2}", a, b, a < b);
        Console.WriteLine("{0} > {1} = {2}", a, b, a > b);
        Console.WriteLine("{0} <= {1} = {2}", a, b, a <= b);
        Console.WriteLine("{0} >= {1} = {2}", a, b, a >= b);
        Console.WriteLine("{0} == {1} = {2}", a, b, a == b);
        Console.WriteLine("{0} != {1} = {2}", a, b, a != b);

        Console.WriteLine("Bitwise operators");
        Console.WriteLine("{0} & {1} = {2}", a, 0xFF, a & 0xFF);
        Console.WriteLine("{0} | {1} = {2}", a, 0xFF, a | 0xFF);
        Console.WriteLine("{0} ^ {1} = {2}", a, 0xFF, a ^ 0xFF);

        Console.WriteLine("Logical operators");
        Console.WriteLine("a > 0 && a < b = {0}", a > 0 && a < b);
        Console.WriteLine("a > 0 || a < b = {0}", a > 0 || a < b);

        Console.WriteLine("Unary operators");
        Console.WriteLine("a-- = {0}", a--);
        Console.WriteLine("a++ = {0}", a++);
        Console.WriteLine("--a = {0}", --a);
        Console.WriteLine("++a = {0}", ++a);
        Console.WriteLine("-a = {0}", -a);
        Console.WriteLine("+b = {0}", +b);
        Console.WriteLine("~a = {0}", ~a);
        Console.WriteLine("!(a > 0 && a < b) = {0}", !(a > 0 && a < b));

        Console.WriteLine("Other operators");
        Console.WriteLine("sizeof(int) = {0}", sizeof(int));
    }

    static void f2()
    {
        uint a = 400, b = 200;
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);

        Console.WriteLine("Multiplicative operators");
        Console.WriteLine("{0} * {1} = {2}", a, b, a * b);
        Console.WriteLine("{0} / {1} = {2}", a, b, a / b);
        Console.WriteLine("{0} % {1} = {2}", a, b, a % b);

        Console.WriteLine("Additive operators");
        Console.WriteLine("{0} + {1} = {2}", a, b, a + b);
        Console.WriteLine("{0} - {1} = {2}", a, b, a - b);

        Console.WriteLine("Shift operators");
        Console.WriteLine("{0} >> {1} = {2}", a, 8, a >> 8);
        Console.WriteLine("{0} << {1} = {2}", b, 8, b << 8);

        Console.WriteLine("Relational operators");
        Console.WriteLine("{0} < {1} = {2}", a, b, a < b);
        Console.WriteLine("{0} > {1} = {2}", a, b, a > b);
        Console.WriteLine("{0} <= {1} = {2}", a, b, a <= b);
        Console.WriteLine("{0} >= {1} = {2}", a, b, a >= b);
        Console.WriteLine("{0} == {1} = {2}", a, b, a == b);
        Console.WriteLine("{0} != {1} = {2}", a, b, a != b);

        Console.WriteLine("Bitwise operators");
        Console.WriteLine("{0} & {1} = {2}", a, 0xFF, a & 0xFF);
        Console.WriteLine("{0} | {1} = {2}", a, 0xFF, a | 0xFF);
        Console.WriteLine("{0} ^ {1} = {2}", a, 0xFF, a ^ 0xFF);

        Console.WriteLine("Logical operators");
        Console.WriteLine("a > 0 && a < b = {0}", a > 0 && a < b);
        Console.WriteLine("a > 0 || a < b = {0}", a > 0 || a < b);

        Console.WriteLine("Unary operators");
        Console.WriteLine("a-- = {0}", a--);
        Console.WriteLine("a++ = {0}", a++);
        Console.WriteLine("--a = {0}", --a);
        Console.WriteLine("++a = {0}", ++a);
        Console.WriteLine("-a = {0}", -a);
        Console.WriteLine("+b = {0}", +b);
        Console.WriteLine("~a = {0}", ~a);
        Console.WriteLine("!(a > 0 && a < b) = {0}", !(a > 0 && a < b));
    }

    static void Main()
    {
        f1();
        f2();
    }
}