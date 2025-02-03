using FlorianMezzo.Pages;

namespace FlorianMezzo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(Compatibility), typeof(Compatibility));
            Routing.RegisterRoute(nameof(HealthCheck), typeof(HealthCheck));
            Routing.RegisterRoute(nameof(MezzoAnalysis), typeof(MezzoAnalysis));
            Routing.RegisterRoute(nameof(More3AM), typeof(More3AM));
        }
    }
}
