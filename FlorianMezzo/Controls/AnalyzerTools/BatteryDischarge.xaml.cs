namespace FlorianMezzo.Controls.AnalyzerTools;
using FlorianMezzo.Controls.db;
using System.Diagnostics;

public partial class BatteryDischarge : ContentView
{
    private Dictionary<string, List<HardwareResourcesData>> averagableBatteryDataBySession;

    public BatteryDischarge()
	{
		InitializeComponent();

        // fetch past battery percentage data
        LocalDbService _dbService;
        _dbService = new LocalDbService();
        Task.Run( async () => {
            this.averagableBatteryDataBySession = await _dbService.GetLatestBatteryData();
        });

        //Populate left side (graph)
        GetAverageDischargeRate();

        //Populate right side (data table)
        SessionDataGrid.Children.Add(GenerateGridForSessionData(averagableBatteryDataBySession.Keys.ToList()[0]));

    }

    //(1) find the rate of discharge per session
    //(2) average the rates finding the overall average rate of discharge
    public double GetAverageDischargeRate()
    {
        Dictionary<string, double> sessionDischargeRates = [];

        //(1)
        foreach (string sessionId in averagableBatteryDataBySession.Keys)
        {
            List<HardwareResourcesData> sessionData = averagableBatteryDataBySession[sessionId];
            double oldBatteryPercent = 0.0;
            double newBatteryPercent = 0.0;
            double totalDeltaBattery = 0.0;

            // find the average rate of discharge within the session
            for (int i = 0; i < sessionData.Count; i++)
            {
                oldBatteryPercent = newBatteryPercent;
                Double.TryParse(sessionData[i].Feedback.Trim(), out newBatteryPercent); // strore first battery percentage

                // if not the first data entry, find difference between this and the last battery percentage. Then add it to the total
                if (i - 1 >= 0)
                {
                    totalDeltaBattery += (oldBatteryPercent - newBatteryPercent);
                }
            }
            var startTime = DateTime.ParseExact(sessionData[0].DateTime, "yyyy-MM-dd HH:mm:ss", null);
            var endTime = DateTime.ParseExact(sessionData[1].DateTime, "yyyy-MM-dd HH:mm:ss", null);

            double deltaMinutes = (endTime - startTime).TotalMinutes;
            sessionDischargeRates.Add(sessionId, totalDeltaBattery / deltaMinutes);

            Debug.WriteLine($"Session Id: {sessionId} ->\n" +
                $"\tDelta Battery\n" +
                $"\tDelta Minutes: {deltaMinutes}\n" +
                $"\tAverage: {sessionDischargeRates[sessionId]}");
        }

        double totalDiscahrgeRates = 0.0;
        //(2)
        foreach (string sessionId in sessionDischargeRates.Keys)
        {
            Debug.WriteLine($"Session Id: {sessionId} -> Average: {sessionDischargeRates[sessionId]}");
            totalDiscahrgeRates += sessionDischargeRates[sessionId];
        }
        double avgPercentDischargedPerMinute = totalDiscahrgeRates / sessionDischargeRates.Keys.Count;
        Debug.WriteLine($"Average Battery Discharge: {avgPercentDischargedPerMinute}");

        MainThread.BeginInvokeOnMainThread(() =>
        {
            averageNumDisplay.Text = $"Average % Discharge\n{avgPercentDischargedPerMinute:F2}%/min";
        });

        return avgPercentDischargedPerMinute;
    }

    // Build table for one dataset
    public Grid GenerateGridForSessionData(string sessionId)
    {
        List<HardwareResourcesData> sessionBatteryData = averagableBatteryDataBySession[sessionId];
        List<string> columnTitles = ["id", "Battery %", "Date Time"];

        // Get label Style
        Resources.TryGetValue("SubHeadline", out var style);

        Grid grid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection(),
            ColumnDefinitions = new ColumnDefinitionCollection()
        };

        // Build Table
            // generate equal-sized columns
        foreach(string columnnTitle in columnTitles)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        }
            // generate equal-sized rows
        grid.RowDefinitions.Add(new RowDefinition { Height = 25 });
        foreach ( HardwareResourcesData entry in sessionBatteryData)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = 25 });
        }

        // Populate labels
        for (int columnNum = 0; columnNum < columnTitles.Count; columnNum++)
        {
            Label label = new Label
            {
                Text = columnTitles[columnNum],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Black,
                Margin = 1
            };
            if(style != null) { label.Style = (Style)style; }
            grid.Children.Add(label);
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, columnNum);
        }
        // Populate grid with data
        for (int entryNum=0; entryNum<sessionBatteryData.Count; entryNum++)
        {
            // build row
            Label iLabel = new Label
            {
                Text = entryNum.ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.LightGray,
                Margin = 2
            };
            Label batteryLabel = new Label
            {
                Text = sessionBatteryData[5].ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.LightGray,
                Margin = 2
            };
            Label dateTimeLabel = new Label
            {
                Text = sessionBatteryData[6].ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.LightGray,
                Margin = 2
            };
            if (style != null) {
                iLabel.Style = (Style)style;
                batteryLabel.Style = (Style)style;
                dateTimeLabel.Style = (Style)style;
            }

            // place row
            grid.Children.Add(iLabel);
            Grid.SetRow(iLabel, entryNum+1);
            Grid.SetColumn(iLabel, 0);
            grid.Children.Add(batteryLabel);
            Grid.SetRow(batteryLabel, entryNum + 1);
            Grid.SetColumn(batteryLabel, 1);
            grid.Children.Add(dateTimeLabel);
            Grid.SetRow(dateTimeLabel, entryNum + 1);
            Grid.SetColumn(dateTimeLabel, 2);
        }

        return grid;
    }
}