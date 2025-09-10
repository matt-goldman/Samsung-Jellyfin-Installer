using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiDialogService : IDialogService
    {
        private Page? GetCurrentPage() => Application.Current?.Windows?.FirstOrDefault()?.Page;

        public async Task ShowMessageAsync(string message)
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                await page.DisplayAlert("Information", message, "OK");
            }
        }

        public async Task ShowErrorAsync(string message)
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                await page.DisplayAlert("Error", message, "OK");
            }
        }

        public async Task<bool> ShowConfirmationAsync(string message)
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                return await page.DisplayAlert("Confirmation", message, "Yes", "No");
            }
            return false;
        }

        public async Task<string?> PromptForIpAsync(string title, string message)
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                return await page.DisplayPromptAsync(title, message, "OK", "Cancel", "Enter IP Address", keyboard: Keyboard.Default);
            }
            return null;
        }
    }
}
