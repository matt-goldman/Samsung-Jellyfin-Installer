namespace Samsung_Jellyfin_Installer.Shared.Services
{
    public interface ISettingsService
    {
        T? GetSetting<T>(string key);
        void SetSetting<T>(string key, T value);
        void Save();
    }
}
