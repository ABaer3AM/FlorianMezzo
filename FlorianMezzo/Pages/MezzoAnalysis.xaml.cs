using System.Windows.Input;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Pages;

public partial class MezzoAnalysis : ContentPage
{
    public ICommand ChangeViewCommand { get; }

    public MezzoAnalysis()
	{
		InitializeComponent();
        BindingContext = new AnalyzerViewModel();
    }


    // Navigation methods
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
}