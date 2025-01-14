using FlorianMezzo.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using FlorianMezzo.Controls.db;

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
