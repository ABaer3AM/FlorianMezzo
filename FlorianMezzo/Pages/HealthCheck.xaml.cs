using System.Diagnostics;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Pages;

public partial class HealthCheck : ContentPage
{
	public HealthCheck()
	{
        InitializeComponent();
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

    /* Overloaded method-
     * (sender,e) from button 
     * () called from another function
     */
    private async void getAllStatuses(object sender, EventArgs e)
    {
        getAllStatuses();
    }
    private async void getAllStatuses()
    {
        await Task.Delay(1000);
        initESDs();
        Debug.WriteLine("getting statuses for health check");
    }


    // Navigation methods
    private async void redirectToInstallGuide(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InstallGuide(), false);
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HealthCheck(), false);
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
}