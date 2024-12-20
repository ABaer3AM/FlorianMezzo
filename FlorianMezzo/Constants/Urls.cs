using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FlorianMezzo.Constants
{
    internal class Urls
    {
        private readonly Dictionary<string, string> tiles;
        private readonly Tuple<string, string>[] coreDependencies;

        public Urls()
        {
            tiles = new Dictionary<string, string> {
                {"wite board", "https://witeboard.com/" },
                {"watchDuty", "https://app.watchduty.org/"},
                {"esriSatellite", "https://www.arcgis.com/apps/mapviewer/index.html?layers=10df2279f9684e4a9f6a7f08febac2a9"},
                {"googleSatellite", "https://www.google.com/maps/"},
                {"google", "https://www.google.com/"},
                {"windy", "https://www.windy.com/-Waves-waves"},
                {"macroWeather", "https://earth.nullschool.net/"}
            };

            coreDependencies = new[]{
                Tuple.Create("Florian Web (Phila)", "https://phila.florian.app/About"),
                Tuple.Create("Florian Web (QA)", "https://qa.florian.app/About"),
                Tuple.Create("Florian Web (Demo)", "https://demo.florian.app/About"),
                Tuple.Create("Florian Web", "https://florian.app/About"),
                Tuple.Create("Bring Maps", "https://www.bing.com/maps"),
                Tuple.Create("Google Play Store", "http://play.google.com"),
                Tuple.Create("Next Nav", "https://api.nextnav.io/")
            };
        }
        public Dictionary<string, string> getTiles()
        {
            return tiles;
        }
        public Tuple<string, string>[] getCoreDependencies()
        {
            return coreDependencies;
        }
    }
}
