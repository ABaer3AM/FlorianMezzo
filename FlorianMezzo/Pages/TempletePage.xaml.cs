namespace FlorianMezzo.Pages;

public partial class TempletePage : ContentPage
{
	public TempletePage()
	{
		InitializeComponent();
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