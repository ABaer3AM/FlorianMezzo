namespace FlorianMezzo.Controls.AnalyzerTools;
using FlorianMezzo.Controls.db;
using System.Diagnostics;
using FlorianMezzo.Controls.AnalyzerTools;

public partial class BatteryDischarge : ContentView
{
    private Dictionary<string, List<HardwareResourcesData>> averagableBatteryDataBySession;
    private int currentSessionI;
    private LineChartDrawable chartDrawable;

    public BatteryDischarge(Dictionary<string, List<HardwareResourcesData>> averagableBatteryDataBySessionIn)
	{
        averagableBatteryDataBySession = averagableBatteryDataBySessionIn;
        currentSessionI = 0;
        InitializeComponent();


        if (averagableBatteryDataBySessionIn.Count == 0)
        {
            return;
        }

        chartDrawable = new LineChartDrawable(300, 200);
        ChartView.Drawable = chartDrawable;

        LoadChartData(300, 200);


        //Populate left side (graph)
        GetAverageDischargeRate();

        // Exit if no data was sent in
        if (averagableBatteryDataBySessionIn == null)
        {
            sessionNum.Text = $"Session Na/Na";
            return;
        }
        else if(averagableBatteryDataBySessionIn.Keys.Count == 1)
        {
            leftSelector.IsVisible = false;
            rightSelector.IsVisible = false;
        }

        //Populate right side (data table)
        SessionDataGrid.Children.Add(GenerateGridForSessionData(averagableBatteryDataBySession.Keys.ToList()[0]));

        sessionNum.Text = $"Session {currentSessionI + 1}/{averagableBatteryDataBySession.Keys.Count}";
    }


    private void LoadChartData(float maxX, float maxY)
    {
        if (averagableBatteryDataBySession[averagableBatteryDataBySession.Keys.First()].Count < 4) { return; }
        int count=0;


        foreach(string sessionId in averagableBatteryDataBySession.Keys.Take(5).ToList())
        {
            count++;
            Debug.WriteLine($"Writing line for session {sessionId}");
            var sessionData = averagableBatteryDataBySession[sessionId];

            var startTime = DateTime.ParseExact(sessionData.First().DateTime, "yyyy-MM-dd HH:mm:ss", null);
            var endTime = DateTime.ParseExact(sessionData.Last().DateTime, "yyyy-MM-dd HH:mm:ss", null);
            double timeMinutes = (endTime - startTime).TotalMinutes;

            List<PointF> line = new List<PointF>();
            foreach(HardwareResourcesData enrty in averagableBatteryDataBySession[sessionId])
            {
                Double.TryParse(enrty.Feedback.Trim(), out double batteryPercent);
                double minutesToEntry = (DateTime.ParseExact(enrty.DateTime, "yyyy-MM-dd HH:mm:ss", null) - startTime).TotalMinutes;

                Debug.WriteLine($"\tnode at: ({(float)((minutesToEntry / timeMinutes) * maxX)},{(float)(batteryPercent)})");
                line.Add(new PointF((float)((minutesToEntry/ timeMinutes)*maxX), (float)((batteryPercent/100)*maxY)));
            }

            Debug.WriteLine("\n");
            chartDrawable.Lines.Add(line);
            chartDrawable.LineNames.Add(count.ToString());
        }

        ChartView.Invalidate(); // Refresh the view
    }



    private void switchToPastSession(object sender, EventArgs e)
    {//rotate the datatable to display the past sessions data
        if (currentSessionI > 0 && currentSessionI <= averagableBatteryDataBySession.Keys.Count)
        {
            SessionDataGrid.Children.Clear();
            currentSessionI--;
            SessionDataGrid.Children.Add(GenerateGridForSessionData(averagableBatteryDataBySession.Keys.ToList()[currentSessionI]));
        }
        if(currentSessionI == 0)
        {
            leftSelector.IsVisible = false;
            rightSelector.IsVisible = true;
        }
        else
        {
            rightSelector.IsVisible = true;
            leftSelector.IsVisible = true;
        }
        sessionNum.Text = $"Session {currentSessionI + 1}/{averagableBatteryDataBySession.Keys.Count}";
    }
    private void switchToNextSession(object sender, EventArgs e)
    {//rotate the datatable to display the next sessions data

        if (currentSessionI < averagableBatteryDataBySession.Keys.Count-1 && currentSessionI >= 0)
        {
            SessionDataGrid.Children.Clear();
            currentSessionI++;
            SessionDataGrid.Children.Add(GenerateGridForSessionData(averagableBatteryDataBySession.Keys.ToList()[currentSessionI]));
        }
        if (currentSessionI == averagableBatteryDataBySession.Keys.Count)
        {
            rightSelector.IsVisible = false;
            leftSelector.IsVisible = true;
        }
        else
        {
            rightSelector.IsVisible = true;
            leftSelector.IsVisible = true;
        }
        sessionNum.Text = $"Session {currentSessionI + 1}/{averagableBatteryDataBySession.Keys.Count}";
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
                    Debug.WriteLine($"\t\tbattery change numer {i}: {oldBatteryPercent - newBatteryPercent}");
                    totalDeltaBattery += (oldBatteryPercent - newBatteryPercent);
                }
            }
            var startTime = DateTime.ParseExact(sessionData[0].DateTime, "yyyy-MM-dd HH:mm:ss", null);
            var endTime = DateTime.ParseExact(sessionData.Last().DateTime, "yyyy-MM-dd HH:mm:ss", null);

            double deltaMinutes = (endTime - startTime).TotalMinutes;
            sessionDischargeRates.Add(sessionId, totalDeltaBattery / deltaMinutes);

            Debug.WriteLine($"Session Id: {sessionId} ->\n" +
                $"\tDelta Battery: {totalDeltaBattery}\n" +
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
            averageNumDisplay.Text = $"Average: {avgPercentDischargedPerMinute:F2}%/min";
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
            ColumnDefinitions = new ColumnDefinitionCollection(),
            Padding = 10
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
                TextColor = Color.FromArgb("#717cba"),
                Margin = 2,
                Padding = 10
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
                BackgroundColor = Color.FromArgb("#404040"),
                Margin = 2,
                Padding = 10
            };
            Label batteryLabel = new Label
            {
                Text = sessionBatteryData[entryNum].Feedback,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromArgb("#404040"),
                Margin = 2,
                Padding = 10
            };
            Label dateTimeLabel = new Label
            {
                Text = sessionBatteryData[entryNum].DateTime,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromArgb("#404040"),
                Margin = 2,
                Padding = 10
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