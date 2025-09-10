using Samsung_Jellyfin_Installer.Shared.Services;

namespace Samsung_Jellyfin_Installer.Services
{
    public class WpfSettingsService : ISettingsService
    {
        public T? GetSetting<T>(string key)
        {
            var value = Settings.Default[key];
            if (value is T typedValue)
            {
                return typedValue;
            }
            
            return default(T);
        }

        public void SetSetting<T>(string key, T value)
        {
            Settings.Default[key] = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}
