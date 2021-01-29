using System;
using System.Threading.Tasks;

namespace AsynchronousConsoleCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            //var exercise = new Exercise_ChangeToAsync();
            //exercise.Execute();


            var example = new ForEachAsync();
            example.Execute();

            Console.ReadKey();
        }
    }
}
