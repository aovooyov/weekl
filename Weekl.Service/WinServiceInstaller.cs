using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Weekl.Service
{
    [RunInstaller(true)]
    public partial class WinServiceInstaller : Installer
    {
        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        public WinServiceInstaller()
        {
            InitializeComponent();

            _serviceInstaller = new ServiceInstaller();
            _processInstaller = new ServiceProcessInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;
            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.ServiceName = "Weekl.Service";

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
