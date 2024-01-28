using Microcharts.Maui;

namespace ProjektCrossplatform;
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
			})
			.UseMicrocharts();

		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddTransient<SampleDataService>();

		builder.Services.AddSingleton<ListDetailPage>();

		builder.Services.AddSingleton<LocalDatabaseService>();

        return builder.Build();
	}
}
