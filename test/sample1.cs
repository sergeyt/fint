using System;

namespace hello
{
    class Program
    {
        static void Foo()
        {
            Console.WriteLine("Foo");
        }

        static string Greeting(string name)
        {
            return "Hello, " + name + "!";
        }

        static void Main(string[] args)
        {
            Foo();
            var s = Greeting("World");
            Console.WriteLine(s);
        }
    }
}
