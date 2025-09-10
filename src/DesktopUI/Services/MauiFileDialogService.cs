using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiFileDialogService : IFileDialogService
    {
        public async Task<string?> OpenFileAsync(string title, string filter)
        {
            try
            {
                var customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.data" } },
                        { DevicePlatform.Android, new[] { "*/*" } },
                        { DevicePlatform.WinUI, new[] { "*" } },
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { "public.data" } },
                    });

                var options = new PickOptions()
                {
                    PickerTitle = title,
                    FileTypes = customFileType,
                };

                var result = await FilePicker.Default.PickAsync(options);
                return result?.FullPath;
            }
            catch (Exception ex)
            {
                // User canceled or an error occurred
                System.Diagnostics.Debug.WriteLine($"File picker error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> SaveFileAsync(string title, string filter, string? defaultFileName = null)
        {
            // MAUI doesn't have a built-in save dialog, but we can use platform-specific implementations
            // For now, we'll use the folder picker and suggest creating the file
            var folderPath = await SelectFolderAsync($"{title} - Select folder to save to");
            if (!string.IsNullOrEmpty(folderPath) && !string.IsNullOrEmpty(defaultFileName))
            {
                return Path.Combine(folderPath, defaultFileName);
            }
            return null;
        }

        public async Task<string?> SelectFolderAsync(string title)
        {
            try
            {
                // MAUI doesn't have a built-in FolderPicker
                // For now, we'll use the file picker and return the directory of the selected file
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    return Path.GetDirectoryName(result.FullPath);
                }
                return null;
            }
            catch (Exception ex)
            {
                // User canceled or an error occurred
                System.Diagnostics.Debug.WriteLine($"Folder picker error: {ex.Message}");
                return null;
            }
        }
    }
}
