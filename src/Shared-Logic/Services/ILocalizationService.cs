namespace Samsung_Jellyfin_Installer.Shared.Services
{
    public interface ILocalizationService
    {
        void ChangeLanguage(string languageCode);
        string GetString(string key);
    }
}
