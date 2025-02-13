namespace FlorianMezzo.Constants
{
    internal class Urls
    {
        private readonly Tuple<string, string>[] tiles;
        private readonly Tuple<string, string>[] coreDependencies;

        public Urls()
        {
            tiles = [
                Tuple.Create("wite board", "https://witeboard.com/"),
                Tuple.Create("watchDuty", "https://app.watchduty.org/"),
                Tuple.Create("esriSatellite", "https://www.arcgis.com/apps/mapviewer/index.html?layers=10df2279f9684e4a9f6a7f08febac2a9"),
                Tuple.Create("googleSatellite", "https://www.google.com/maps/"),
                Tuple.Create("google", "https://www.google.com/"),
                Tuple.Create("windy", "https://www.windy.com/-Waves-waves"),
                Tuple.Create("macroWeather", "https://earth.nullschool.net/")
            ];

            coreDependencies = [
                Tuple.Create("Florian Web (Phila)", "https://phila.florian.app/About"),
                Tuple.Create("Florian Web (QA)", "https://qa.florian.app/About"),
                Tuple.Create("Florian Web (Demo)", "https://demo.florian.app/About"),
                Tuple.Create("Florian Web", "https://florian.app/About"),
                Tuple.Create("Bring Maps", "https://www.bing.com/maps"),
                Tuple.Create("Google Play Store", "http://play.google.com")
            ];
        }
        public Tuple<string, string>[] getTiles()
        {
            return tiles;
        }
        public Tuple<string, string>[] getCoreDependencies()
        {
            return coreDependencies;
        }
    }
}
