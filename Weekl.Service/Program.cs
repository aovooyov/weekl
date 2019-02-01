using System.ServiceProcess;
using System.Threading;
using Nito.AsyncEx;

namespace Weekl.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var service = new WinService();
            AsyncContext.Run(service.RunByTasksAsync);
            Thread.Sleep(Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WinService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
