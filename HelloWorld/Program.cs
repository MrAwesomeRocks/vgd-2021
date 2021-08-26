using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HelloWorld!");

            Program2 x = new();
            x.printStr();
        }
    }
    class Program2
    {
        public void printStr()
        {
            Console.WriteLine("HelloWorld2!");
        }
    }
}
