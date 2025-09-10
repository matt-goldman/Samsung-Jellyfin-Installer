using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DesktopUI.Services;
using Samsung_Jellyfin_Installer.Shared.Services;
using Samsung_Jellyfin_Installer.Shared.ViewModels;

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
			});

		builder.Services.AddMauiBlazorWebView();

		// Register shared services with MAUI implementations
		builder.Services.AddSingleton<IDialogService, MauiDialogService>();
		builder.Services.AddSingleton<IUIThreadService, MauiUIThreadService>();
		builder.Services.AddSingleton<ISettingsService, MauiSettingsService>();
		builder.Services.AddSingleton<IFileDialogService, MauiFileDialogService>();
		builder.Services.AddSingleton<ILocalizationService, MauiLocalizationService>();
		builder.Services.AddSingleton<INetworkService, MauiNetworkService>();
		builder.Services.AddSingleton<ITizenInstallerService, MauiTizenInstallerService>();

		// Register shared ViewModels
		builder.Services.AddTransient<JellyfinConfigViewModel>();
		builder.Services.AddTransient<MainWindowViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
