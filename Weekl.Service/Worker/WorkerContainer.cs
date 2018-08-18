using System.ComponentModel;
using Ninject;

namespace Weekl.Service.Worker
{
    public class WorkerContainer
    {
        private static IKernel _container;

        private WorkerContainer()
        {

        }

        public static IKernel Current => _container ?? (_container = new StandardKernel(new WorkerModule()));
    }
}