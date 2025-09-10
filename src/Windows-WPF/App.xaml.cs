using Microsoft.Extensions.DependencyInjection;
using Samsung_Jellyfin_Installer.Services;
using Samsung_Jellyfin_Installer.ViewModels;
using Samsung_Jellyfin_Installer.WPF.Services;
using Samsung_Jellyfin_Installer.Shared.Services;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows;

namespace Samsung_Jellyfin_Installer
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register HttpClient with custom configuration
            services.AddSingleton(provider =>
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("SamsungJellyfinInstaller/1.0");
                client.Timeout = TimeSpan.FromMinutes(2);
                return client;
            });

            // Register services
            services.AddSingleton<Samsung_Jellyfin_Installer.Shared.Services.IDialogService, WpfDialogService>();
            services.AddSingleton<Samsung_Jellyfin_Installer.Shared.Services.IUIThreadService, WpfUIThreadService>();
            services.AddSingleton<Samsung_Jellyfin_Installer.Services.ITizenInstallerService, Samsung_Jellyfin_Installer.Services.TizenInstallerService>();
            services.AddSingleton<Samsung_Jellyfin_Installer.Services.INetworkService, Samsung_Jellyfin_Installer.Services.NetworkService>();

            // Register ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<SettingsViewModel>();

            // Register Views
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CleanOldSettings();
            string savedLanguage = Settings.Default.Language ?? "en";

            var culture = new System.Globalization.CultureInfo(savedLanguage);

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;


            Samsung_Jellyfin_Installer.Services.LocalizedStrings.Instance.ChangeLanguage(savedLanguage);

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Application failed to start: {ex.Message}");
                MessageBox.Show($"Application failed to start: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);

                Shutdown();
            }
        }

        public static void CleanOldSettings()
        {
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Samsung-Jellyfin-Installe");

            if (!Directory.Exists(basePath))
                return;

            string exeName = Assembly.GetExecutingAssembly().GetName().Name;
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            try
            {
                var dirs = Directory.GetDirectories(basePath);

                foreach (var dir in dirs)
                {
                    if (dir.Contains(currentVersion))
                        continue;

                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to delete directory {dir}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during old settings cleanup: {ex.Message}");
            }
        }
    }
}