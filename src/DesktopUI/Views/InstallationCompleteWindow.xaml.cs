namespace Samsung_Jellyfin_Installer.DesktopUI.Views
{
    public partial class InstallationCompleteWindow : ContentPage
    {
        public InstallationCompleteWindow()
        {
            InitializeComponent();
        }

        private async void Validation_Click(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync("https://ko-fi.com/patrickst");
            }
            catch (Exception ex)
            {
                // Handle error opening URL
                System.Diagnostics.Debug.WriteLine($"Failed to open URL: {ex.Message}");
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void CloseButton_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
