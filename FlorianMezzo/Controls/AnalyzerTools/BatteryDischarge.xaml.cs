namespace FlorianMezzo.Controls.AnalyzerTools;
using FlorianMezzo.Controls.db;
using System.Diagnostics;

public partial class BatteryDischarge : ContentView
{
	private LocalDbService _dbService;
	public BatteryDischarge()
	{
		InitializeComponent();
        _dbService = new LocalDbService();

        FetchBatteryData();

    }

    //(1) fetch past battery percentage data
    //(2) find the rate of discharge per session
    //(3) average the rates finding the overall average rate of discharge
    public async void FetchBatteryData()
    {
        //(1)
        Dictionary<string, List<HardwareResourcesData>> batteryDataBySession = await _dbService.GetLatestBatteryData();

        Dictionary<string, double> sessionDischargeRates = [];

        //(2)
        foreach (string sessionId in batteryDataBySession.Keys)
        {
            List<HardwareResourcesData> sessionData = batteryDataBySession[sessionId];
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
        }

        double totalDiscahrgeRates = 0.0;
        //(3)
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
    }
}