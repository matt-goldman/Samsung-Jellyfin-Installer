using Samsung_Jellyfin_Installer.Shared.Services;

namespace Samsung_Jellyfin_Installer.DesktopUI.Views
{
    public partial class IpInputDialog : ContentPage
    {
        public string? EnteredIp { get; private set; }
        private readonly IDialogService _dialogService;

        public IpInputDialog(string title, string message, IDialogService dialogService)
        {
            InitializeComponent();
            _dialogService = dialogService;
            Title = title;
            PromptText.Text = message;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            EnteredIp = InputBox.Text;
            await Shell.Current.GoToAsync("..");
        }

        private async void Cancel_Click(object sender, EventArgs e)
        {
            EnteredIp = null;
            await Shell.Current.GoToAsync("..");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InputBox.Focus();
        }
    }
}
