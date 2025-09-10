namespace Samsung_Jellyfin_Installer.Shared.Services
{
    public interface IFileDialogService
    {
        Task<string?> OpenFileAsync(string title, string filter);
        Task<string?> SaveFileAsync(string title, string filter, string? defaultFileName = null);
        Task<string?> SelectFolderAsync(string title);
    }
}
