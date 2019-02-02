using System;
using System.Text;
using System.Threading;

namespace Weekl.ConsoleApp
{
    class Program
    {
        private static readonly WeeklConsole Weekl = new WeeklConsole();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            var task = Weekl.SyncFeedAsync();
            task.Wait();

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}
