using FlorianMezzo.Pages;

namespace FlorianMezzo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(InstallGuide), typeof(InstallGuide));
            Routing.RegisterRoute(nameof(HealthCheck), typeof(HealthCheck));
            Routing.RegisterRoute(nameof(ItHandOff), typeof(ItHandOff));
            Routing.RegisterRoute(nameof(FlorianBTS), typeof(FlorianBTS));
            Routing.RegisterRoute(nameof(More3AM), typeof(More3AM));
        }
    }
}
