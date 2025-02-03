using FlorianMezzo.Pages;
using System.ComponentModel;
using FlorianMezzo.Constants;
using System.Diagnostics;

namespace FlorianMezzo
{
    public partial class MainPage : ContentPage
    {
        public List<string> Options { get; set; }
        public string SelectedOption { get; set; }
        private int _checkIntHr;
        private int _checkIntMin;
        private int _checkIntSec;
        private AppSettings Settings = new AppSettings();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            // Read settings file
            Settings.LoadOrCreateSettings();
            _checkIntHr = Settings.Interval / 3600;
            _checkIntMin = Settings.Interval % 3600 / 60;
            _checkIntSec = Settings.Interval % 60;

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


        // Time Inteval Methods -------------------------------------------------------------------
        public int CheckIntHr
        {
            get => _checkIntHr;
            set
            {
                if (_checkIntHr != value)
                {
                    _checkIntHr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_checkIntHr)));
                }
            }
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
                ; if (_checkIntSec != value)
                {
                    _checkIntSec = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckIntSec)));
                }
            }
        }
        private void setInterval(object sender, EventArgs e)
        {
            Debug.WriteLine($"Changed Interval from {Settings.Interval} to {(_checkIntHr * 3600) + (_checkIntMin * 60) + _checkIntSec}");
            Settings.UpdateInterval((_checkIntHr * 3600) + (_checkIntMin * 60) + _checkIntSec);
        }
        // ----------------------------------------------------------------------------------------

    }

}
