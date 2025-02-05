using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FlorianMezzo.Controls.AnalyzerTools;

namespace FlorianMezzo.Controls
{
    public partial class AnalyzerViewModel : INotifyPropertyChanged
    {

        private View _mainView;

        public View MainView
        {
            get => _mainView;
            set
            {
                _mainView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeViewCommand { get; }

        public AnalyzerViewModel()
        {
            ChangeViewCommand = new Command<string>(ChangeView);
            MainView = new Label { Text = "Will be replaced with a tool view", FontSize = 24, HorizontalOptions = LayoutOptions.Center };
        }

        private void ChangeView(string viewName)
        {
            switch (viewName)
            {
                case "batteryDischarge":
                    MainView = new BatteryDischarge();
                    break;
                case "ezriMapSim":
                    MainView = new Label { Text = "This is View 2", FontSize = 24, TextColor = Colors.Green };
                    break;
                case "View3":
                    MainView = new Label { Text = "This is View 3", FontSize = 24, TextColor = Colors.Red };
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
