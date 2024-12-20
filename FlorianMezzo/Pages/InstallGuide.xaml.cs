using System.Collections.ObjectModel;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Pages;

public partial class InstallGuide : ContentPage
{

    public InstallGuide()
	{
		InitializeComponent();

        // Create main StateDisplay
        var mainStateDisplay = new StateDisplay
        {
            Title = "Main Status",
            Status = 1,
            Feedback = "Everything is good."
        };

        // Create dropdown StateDisplays
        var stateDisplays = new List<StateDisplay>
        {
            new StateDisplay { Title = "Sub-item 1", Status = 1, Feedback = "Operational" },
            new StateDisplay { Title = "Sub-item 2", Status = -1, Feedback = "Warning" },
            new StateDisplay { Title = "Sub-item 3", Status = 0, Feedback = "Offline" }
        };

        // Assign to ExpandableStateDisplay
        TestExpandableStateDisplay.MainStateDisplay = mainStateDisplay;
        TestExpandableStateDisplay.StateDisplays = stateDisplays;

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