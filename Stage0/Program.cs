// See https://aka.ms/new-console-template for more information
namespace exe1
{
    class Program
    {
        static void Main(string[] args)
        {
            NewMethod();

            Console.ReadKey();
        }

        private static void NewMethod()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine($"{name} , welcome to my first console application");
        }
    }
}

