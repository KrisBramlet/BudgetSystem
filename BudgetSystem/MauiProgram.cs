using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui;

namespace BudgetSystem;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhBYVBpR2NbfE54flRPal9YVBYiSV9jS31TcUdkW39feXdcQ2RUVQ==");
        builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Solid-900", "FaSolid");
            });

		string dbPath = FileAccessHelper.GetLocalFilePath("Expenses.db3");
		builder.Services.AddSingleton<ExpenseRepository>(s => ActivatorUtilities.CreateInstance<ExpenseRepository>(s,dbPath));
        builder.Services.AddSingleton<ExpenseRepositoryNA>(s => ActivatorUtilities.CreateInstance<ExpenseRepositoryNA>(s, dbPath));
        builder.Services.AddSingleton<CategoryRepository>(s => ActivatorUtilities.CreateInstance<CategoryRepository>(s,dbPath));
        builder.Services.AddSingleton<CategoryRepositoryNA>(s => ActivatorUtilities.CreateInstance<CategoryRepositoryNA>(s, dbPath));

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
