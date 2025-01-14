using FlorianMezzo.Controls.db;
using System.Diagnostics;

namespace FlorianMezzo
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Instantiate Health Check Service in the background
            HealthCheckService _healthCheckService = serviceProvider.GetService<HealthCheckService>();
            if (_healthCheckService != null)
            {
                Debug.WriteLine("Health Check Service is NULL");
            }
            _healthCheckService?.Start();

            MainPage = new AppShell();
        }
    }
}
