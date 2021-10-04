using System;

namespace Targil0
{
   partial class Program
    {
        static void Main(string[] args)
        {
            Welcome1070();
            Welcome1239();
            Console.ReadKey();
        }


        static partial void Welcome1239();
        private static void Welcome1070()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
