using FlorianMezzo.Pages;
using System.ComponentModel;
using FlorianMezzo.Constants;
using System.Diagnostics;
using Microsoft.Maui.Devices.Sensors;

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
            // Read settings file & set 
            Task.Run( async () =>{

                try
                {
                    await Settings.LoadOrCreateSettings();
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                _checkIntHr = Settings.Interval / 3600;
                _checkIntMin = Settings.Interval % 3600 / 60;
                _checkIntSec = Settings.Interval % 60;

                Options = new List<string>{
                    "QA",
                    "CX",
                    "IT",
                    "Sales"
                };

                SelectedOption = "QA";
            });

            InitializeComponent();
            LocatioinPermissionPrompt();
            BindingContext = this;
        }
        // Abstrack method that is defined in platform specific code ------------------------------
        private partial void LocatioinPermissionPrompt();

        // ----------------------------------------------------------------------------------------

        // Navigation Methods ---------------------------------------------------------------------
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
        // ----------------------------------------------------------------------------------------


        private async void showButtonPressed(Button btn)
        {   // shrink and unshrink a button being pressed to give feedback to the user that the button was pressed
            // Shrink effect
            await btn.ScaleTo(0.925, 50);
            // Restore size
            await btn.ScaleTo(1, 50);
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
            showButtonPressed((Button)sender);
            Debug.WriteLine($"Changed Interval from {Settings.Interval} to {(_checkIntHr * 3600) + (_checkIntMin * 60) + _checkIntSec}");
            Settings.UpdateInterval((_checkIntHr * 3600) + (_checkIntMin * 60) + _checkIntSec);
        }
        // ----------------------------------------------------------------------------------------

    }

}
