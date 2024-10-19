using Combat_Critters_2._0.Pages;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Combat_Critters_2._0;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
		Console.WriteLine("[MyApp] Application is starting...");
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("HeaderText.ttf", "CombatCritterLogo");
                fonts.AddFont("Roboto-Regular.ttf", "CardFont1");
                fonts.AddFont("MtgBold.ttf", "CardFront2");
                fonts.AddFont("Chalkduster.ttf", "FlyoutItemFont");
            });

        // //Register ClientSingleton as a singleton service
        // builder.Services.AddSingleton<IClient>(provider => ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

        // // Register BackendService as a singleton service
        // builder.Services.AddSingleton<BackendService>();

        // // Register LoginViewModel as a transient service
        // builder.Services.AddTransient<LoginViewModel>();

        // // Register LoginPage and other pages
        // builder.Services.AddTransient<LoginPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }

    
}
