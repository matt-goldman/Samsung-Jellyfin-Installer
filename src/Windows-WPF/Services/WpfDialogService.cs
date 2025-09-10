using Samsung_Jellyfin_Installer.Shared.Services;
using System.Windows;

namespace Samsung_Jellyfin_Installer.WPF.Services
{
    /// <summary>
    /// WPF-specific implementation of dialog service using MessageBox
    /// </summary>
    public class WpfDialogService : IDialogService
    {
        public async Task<bool> ShowConfirmationAsync(string message)
        {
            return await Task.FromResult(
                MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes
            );
        }

        public async Task ShowErrorAsync(string message)
        {
            await Task.Run(() =>
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            );
        }

        public async Task ShowMessageAsync(string message)
        {
            await Task.Run(() =>
                MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information)
            );
        }

        public async Task<string?> PromptForIpAsync(string title, string message)
        {
            // For WPF, we'll use a simple approach since there's no built-in input dialog
            // Note: This requires Microsoft.VisualBasic reference for InputBox
            var result = Microsoft.VisualBasic.Interaction.InputBox(message, title, string.Empty);
            return await Task.FromResult(string.IsNullOrEmpty(result) ? null : result);
        }
    }
}
