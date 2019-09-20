using System.Runtime.CompilerServices;

namespace hello
{
    class Program
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        static extern void Print(string s);

        static void Main(string[] args)
        {
            Print("Hello World!");
        }
    }
}
