using Microsoft.Win32;
using Samsung_Jellyfin_Installer.Shared.Services;
using System.IO;

namespace Samsung_Jellyfin_Installer.Services
{
    public class WpfFileDialogService : IFileDialogService
    {
        public Task<string?> OpenFileAsync(string title, string filter)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            var result = dialog.ShowDialog();
            return Task.FromResult(result == true ? dialog.FileName : null);
        }

        public Task<string?> SaveFileAsync(string title, string filter, string? defaultFileName = null)
        {
            var dialog = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
                FileName = defaultFileName ?? string.Empty
            };

            var result = dialog.ShowDialog();
            return Task.FromResult(result == true ? dialog.FileName : null);
        }

        public Task<string?> SelectFolderAsync(string title)
        {
            // WPF doesn't have a built-in folder browser dialog
            // This would need to use Windows Forms or a third-party library
            var dialog = new OpenFileDialog
            {
                Title = title,
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection"
            };

            var result = dialog.ShowDialog();
            return Task.FromResult(result == true ? Path.GetDirectoryName(dialog.FileName) : null);
        }
    }
}
