using System.Diagnostics;
using FlorianMezzo.Controls;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Pages;

public partial class HealthCheck : ContentPage
{

    private readonly HealthCheckService _healthCheckService;
    public HealthCheck(HealthCheckService healthCheckService)
	{
        InitializeComponent();
        _healthCheckService = healthCheckService;
        Debug.WriteLine("HealthChecker Page Initialized");
        initESDs();
    }

    private async void initESDs()
    {
        await MainThread.InvokeOnMainThreadAsync(() => {
            softDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "Fetching...", -2);
            resourceESD.MainStateDisplay = new StateDisplay("Hardware Resources", "Fetching...", -2);
        });
    }

    // Health Check Service interactions--------------------------------------
    private void StartService(object sender, EventArgs e)
    {
        _healthCheckService?.Start();
    }
    private void StopService(object sender, EventArgs e)
    {
        _healthCheckService?.Stop();
    }
    // -----------------------------------------------------------------------

    // Navigation methods-----------------------------------------------------
    private async void redirectToInstallGuide(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InstallGuide(), false);
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        // Resolve HealthCheck page from the service provider
        var healthCheckPage = App.Services.GetService<HealthCheck>();
        if (healthCheckPage != null)
        {
            await Navigation.PushAsync(healthCheckPage, false);
        }
    }
    private async void redirectToITHandOff(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ItHandOff(), false);
    }
    private async void redirectToFlorianBTS(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FlorianBTS(), false);
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new More3AM(), false);
    }
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage(), false);
    }
    // -----------------------------------------------------------------------
}