using System.Collections.ObjectModel;
using System.Diagnostics;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Pages;

public partial class InstallGuide : ContentPage
{

    public InstallGuide()
	{
		InitializeComponent();
        initESDs();

        getAllStatuses();
    }



    private async void OpenFlorianInStore(object sender, EventArgs e)
    {
        // Microsoft Store URL for the app
        var storeUrl = "https://www.microsoft.com/store/apps/9p93s9wb325x";
        var storeAppUrl = "ms-windows-store://pdp/?ProductId=9p93s9wb325x";

        // Open the URL using the Launcher
        if (Uri.IsWellFormedUriString(storeUrl, UriKind.Absolute))
        {
            await Launcher.OpenAsync(new Uri(storeAppUrl));
        }
        else
        {
            if (Uri.IsWellFormedUriString(storeUrl, UriKind.Absolute))
            {
                await Launcher.OpenAsync(new Uri(storeUrl));
            }
            else
            {
                await DisplayAlert("Error", "Invalid Store URL", "OK");
            }
        }
    }

    private void initESDs()
    {
        softDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "Fetching...", -2);
    }

    private async void getStatusOfSD()
    {
        UrlChecker urlCheckerObj = new UrlChecker();

        // Update visual to show fetch is running
        Debug.WriteLine($"Started Task running at {DateTime.Now}");

        // run test
        Tuple<StateDisplay, List<StateDisplay>> sdStates = await urlCheckerObj.testSoftDependencies();

        await MainThread.InvokeOnMainThreadAsync(() => {
            softDependencyESD.MainStateDisplay = sdStates.Item1;
            softDependencyESD.StateDisplays = sdStates.Item2;
        });
    }
    
    private void getAllStatuses(object sender, EventArgs e)
    {
        getStatusOfSD();
    }
    private void getAllStatuses()
    {
        getStatusOfSD();
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