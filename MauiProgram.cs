using Microsoft.Extensions.Logging;
using DGSWManager.Data;
using DGSWManager.Views.company;

namespace DGSWManager;

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
			});
		builder.Services.AddSingleton<CompanyListPage>();
		builder.Services.AddTransient<CompanyRegister>();
		
		builder.Services.AddSingleton<CorprationDatabase>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
