using System;

namespace hello
{
    class Program
    {
        static string Greeting(string name)
        {
            return "Hello, " + name + "!";
        }

        static void Main(string[] args)
        {
            var s = Greeting("World");
            Console.WriteLine(s);
        }
    }
}
