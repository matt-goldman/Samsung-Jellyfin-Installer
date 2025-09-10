namespace Samsung_Jellyfin_Installer.DesktopUI.Views
{
    public partial class InstallingWindow : ContentPage
    {
        public InstallingWindow()
        {
            InitializeComponent();
        }

        public void SetStatusText(string message)
        {
            StatusTextBlock.Text = message;
        }
    }
}
