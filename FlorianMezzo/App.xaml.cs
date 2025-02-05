using FlorianMezzo.Controls.db;
using System.Diagnostics;

namespace FlorianMezzo
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            Services = serviceProvider; // Store the service provider for later use

            // Instantiate Health Check Service in the background
            HealthCheckService _healthCheckService = serviceProvider.GetService<HealthCheckService>();
            /*
            if (_healthCheckService != null)
            {
                Debug.WriteLine("Health Check Service is NULL");
            }
            */

            MainPage = new AppShell();
        }
    }
}
