using Microsoft.Extensions.Logging;
using Samsung_Jellyfin_Installer.Shared.Services;
using Samsung_Jellyfin_Installer.Shared.ViewModels;
using DesktopUI.Services;

namespace DesktopUI;

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

			// Register shared services with MAUI implementations
		builder.Services.AddSingleton<IDialogService, MauiDialogService>();
		builder.Services.AddSingleton<IUIThreadService, MauiUIThreadService>();
		builder.Services.AddSingleton<ISettingsService, MauiSettingsService>();
		builder.Services.AddSingleton<IFileDialogService, MauiFileDialogService>();
		builder.Services.AddSingleton<ILocalizationService, MauiLocalizationService>();
		builder.Services.AddSingleton<INetworkService, Samsung_Jellyfin_Installer.Shared.Services.NetworkService>();
		builder.Services.AddSingleton<ITizenInstallerService, MauiTizenInstallerService>();

		// Register shared ViewModels
		builder.Services.AddTransient<JellyfinConfigViewModel>();
		builder.Services.AddTransient<MainWindowViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
