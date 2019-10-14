using System;

class Constants
{
    private const UInt32 MyUInt32_3 = 4294967295;

    static void Main()
    {
        Console.WriteLine(-1);
        Console.WriteLine(0);
        Console.WriteLine(1);
        Console.WriteLine(2);
        Console.WriteLine(3);
        Console.WriteLine(4);
        Console.WriteLine(5);
        Console.WriteLine(6);
        Console.WriteLine(7);
        Console.WriteLine(8);
        Console.WriteLine(9);
        Console.WriteLine(10);
        Console.WriteLine(100);
        Console.WriteLine(1000);
        Console.WriteLine(-1000);

        Console.WriteLine(MyUInt32_3);

        Console.WriteLine(true);
        Console.WriteLine(false);

        Console.WriteLine(3.14f);
        Console.WriteLine(3.14);

        object obj = null;
        Console.WriteLine(obj);

        Console.WriteLine("-- Min/Max");
        Console.WriteLine(sbyte.MinValue);
        Console.WriteLine(sbyte.MaxValue);
        Console.WriteLine(byte.MinValue);
        Console.WriteLine(byte.MaxValue);
        Console.WriteLine(short.MinValue);
        Console.WriteLine(short.MaxValue);
        Console.WriteLine(ushort.MinValue);
        Console.WriteLine(ushort.MaxValue);
        Console.WriteLine(int.MinValue);
        Console.WriteLine(int.MaxValue);
        Console.WriteLine(uint.MinValue);
        Console.WriteLine(uint.MaxValue);
    }
}