using Microsoft.Extensions.Logging;

namespace Combat_Critters_2._0;

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
				fonts.AddFont("HeaderText.ttf", "CombatCritterLogo");
				fonts.AddFont("Roboto-Regular.ttf", "CardFont1");
				fonts.AddFont("MtgBold.ttf", "CardFront2");

			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
