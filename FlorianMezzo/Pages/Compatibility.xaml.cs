using System.Diagnostics;
using FlorianMezzo.Controls;
using FlorianMezzo.Constants;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Pages;

public partial class Compatibility : ContentPage
{
    private AppSettings Settings = new AppSettings();

    public Compatibility()
	{
		InitializeComponent();
        InitStateDisplays();
        Settings.LoadOrCreateSettings();
        UpdateStateDisplays();
    }



    private async void OpenFlorianInStore(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
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

    private void InitStateDisplays()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // state displays
            tileSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies (Workspace Tiles)", "Unfetched", 0);
            coreSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "Unfetched", 0);
            resourceESD.MainStateDisplay = new StateDisplay("Hardware Resources", "Unfetched", 0);
            florianSD.Title = "FLORIAN App";
            florianSD.UpdateFull(0, "--");
        });
    }


    /* Overloaded method-
     * (sender,e) from button 
     * () called from another function
     */
    private async void UpdateStateDisplays(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
        UpdateStateDisplays();
    }
    private async void UpdateStateDisplays()
    {
        string groupId = Settings.LastGroupId;

        // if there is no valid group ID, exit
        if (groupId == "") { return; }

        // Fetch batched data
        LocalDbService dbService =new LocalDbService();
        Dictionary<string, List<DbData>> statuses = await dbService.GetByGroupId(groupId);


        // retrieve Florian data
        DbData florianData = await dbService.GetByGroupIdAndTitle(groupId, "FLORIAN");

        // On main thread, update UI
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // state displays
            tileSoftDependencyESD.UpdateDropdownContent(statuses["tileSoftDependencies"]);
            coreSoftDependencyESD.UpdateDropdownContent(statuses["coreSoftDependencies"]);
            resourceESD.UpdateDropdownContent(statuses["hardwareResources"].Take(statuses["hardwareResources"].Count - 1).ToList());    // omit the last piece of data because it is shown in its own state display

            if (florianData != null)
            {
                florianSD.UpdateFull(florianData.Status, florianData.Feedback);
            }
            else
            {
                florianSD.UpdateFull(0, "Florian Data not found");
            }
        });
    }


    private async void showButtonPressed(Button btn)
    {   // shrink and unshrink a button being pressed to give feedback to the user that the button was pressed
        // Shrink effect
        await btn.ScaleTo(0.925, 50);
        // Restore size
        await btn.ScaleTo(1, 50);
    }


    // Navigation methods-----------------------------------------------------
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
    private async void redirectToCompatibility(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
        await Shell.Current.GoToAsync(nameof(Compatibility));
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
        await Shell.Current.GoToAsync(nameof(HealthCheck));
    }
    private async void redirectToMezzoAnalysis(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
        await Shell.Current.GoToAsync(nameof(MezzoAnalysis));
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        showButtonPressed((Button)sender);
        await Shell.Current.GoToAsync(nameof(More3AM));
    }
    // -----------------------------------------------------------------------
}