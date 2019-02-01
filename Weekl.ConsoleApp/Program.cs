using System;
using Weekl.Core.Helper;
using Weekl.Core.Models;

namespace Weekl.ConsoleApp
{
    class Program
    {
        private static readonly WeeklConsole _weekl = new WeeklConsole();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            var task = _weekl.ReadFeed();
            task.Wait();

            Console.ReadKey();
        }
    }
}
