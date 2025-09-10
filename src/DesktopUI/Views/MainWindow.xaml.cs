using Samsung_Jellyfin_Installer.Shared.ViewModels;

namespace Samsung_Jellyfin_Installer.DesktopUI.Views
{
    public partial class MainWindow : ContentPage
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void Hyperlink_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync("https://patrickst1991.github.io/Samsung-Jellyfin-Installer/");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to open URL: {ex.Message}");
            }
        }
    }
}
