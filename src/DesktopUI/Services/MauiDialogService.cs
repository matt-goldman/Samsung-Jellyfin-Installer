using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiDialogService : IDialogService
    {
        public async Task ShowMessageAsync(string message)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Information", message, "OK");
            }
        }

        public async Task ShowErrorAsync(string message)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
            }
        }

        public async Task<bool> ShowConfirmationAsync(string message)
        {
            if (Application.Current?.MainPage != null)
            {
                return await Application.Current.MainPage.DisplayAlert("Confirmation", message, "Yes", "No");
            }
            return false;
        }

        public async Task<string?> PromptForIpAsync(string title, string message)
        {
            if (Application.Current?.MainPage != null)
            {
                return await Application.Current.MainPage.DisplayPromptAsync(title, message, "OK", "Cancel", "Enter IP Address", keyboard: Keyboard.Default);
            }
            return null;
        }
    }
}
