namespace FlorianMezzo.Pages;

public partial class ItHandOff : ContentPage
{
	public ItHandOff()
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
}