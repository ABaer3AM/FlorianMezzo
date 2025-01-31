using Microsoft.Extensions.Logging;
using FlorianMezzo.Pages;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Oswald-Regular.ttf", "OswaldRegular");
                    fonts.AddFont("Oswald-SemiBold.ttf", "OswaldSemiBold");
                    fonts.AddFont("Barlow-Regular.ttf", "Barlow");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // DB service
            builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddTransient<HealthCheckService>();

            // Page Definitions
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<InstallGuide>();
            builder.Services.AddSingleton<HealthCheck>();

            builder.Services.AddTransient<ItHandOff>();
            builder.Services.AddTransient<FlorianBTS>();
            builder.Services.AddTransient<More3AM>();

            return builder.Build();
        }
    }
}
