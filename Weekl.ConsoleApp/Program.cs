using System;

namespace Weekl.ConsoleApp
{
    class Program
    {
        private static readonly WeeklConsole _weekl = new WeeklConsole();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            _weekl.SyncFeed();

            Console.ReadKey();
        }
    }
}
