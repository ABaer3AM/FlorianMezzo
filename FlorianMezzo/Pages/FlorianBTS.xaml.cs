namespace FlorianMezzo.Pages;

public partial class FlorianBTS : ContentPage
{
	public FlorianBTS()
	{
		InitializeComponent();
	}


    // Navigation methods
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
    private async void redirectToInstallGuide(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InstallGuide));
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HealthCheck));
    }
    private async void redirectToITHandOff(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ItHandOff));
    }
    private async void redirectToFlorianBTS(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FlorianBTS));
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(More3AM));
    }
}