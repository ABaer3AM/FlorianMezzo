namespace FlorianMezzo.Pages;

public partial class TempletePage : ContentPage
{
	public TempletePage()
	{
		InitializeComponent();
	}


    // Navigation methods-----------------------------------------------------
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
    private async void redirectToCompatibility(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Compatibility));
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HealthCheck));
    }
    private async void redirectToMezzoAnalysis(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MezzoAnalysis));
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(More3AM));
    }
    // -----------------------------------------------------------------------
}