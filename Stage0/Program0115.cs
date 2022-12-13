// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

namespace stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome0115();
            welcome0921();
            Console.ReadKey();

        }
        static partial void welcome0921();

        private static void welcome0115()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? "null";
            Console.WriteLine(name + ", welcome to my first console application");
        }
    }
}