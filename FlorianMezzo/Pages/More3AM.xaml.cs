namespace FlorianMezzo.Pages;

public partial class More3AM : ContentPage
{
	public More3AM()
	{
		InitializeComponent();
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