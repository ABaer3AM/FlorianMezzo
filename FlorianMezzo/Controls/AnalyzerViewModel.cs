using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                    MainView = new Label { Text = "This is View 1", FontSize = 24, TextColor = Colors.Blue };
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
