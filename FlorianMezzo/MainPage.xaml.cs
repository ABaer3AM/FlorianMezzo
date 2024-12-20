using FlorianMezzo.Pages;
using System.ComponentModel;

namespace FlorianMezzo
{
    public partial class MainPage : ContentPage
    {
        public List<string> Options { get; set; }
        public string SelectedOption { get; set; }
        private int _checkIntMin;
        private int _checkIntSec;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            InitializeComponent();

            Options = new List<string>{
                "QA",
                "CX",
                "IT",
                "Sales"
            };

            SelectedOption = "QA";

            BindingContext = this;
        }

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


        public int CheckIntMin
        {
            get => _checkIntMin;
            set
            {
                if (_checkIntMin != value)
                {
                    _checkIntMin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckIntMin)));
                }
            }
        }
        public int CheckIntSec
        {
            get => _checkIntSec;
            set
            {
                if (_checkIntSec != value)
                {
                    _checkIntSec = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckIntSec)));
                }
            }
        }
    }

}
