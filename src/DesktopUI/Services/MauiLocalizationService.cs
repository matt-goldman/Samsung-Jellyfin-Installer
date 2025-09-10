using Samsung_Jellyfin_Installer.Shared.Services;
using System.Globalization;

namespace DesktopUI.Services
{
    public class MauiLocalizationService : ILocalizationService
    {
        private CultureInfo _currentCulture = CultureInfo.CurrentCulture;

        public void ChangeLanguage(string languageCode)
        {
            try
            {
                var culture = new CultureInfo(languageCode);
                _currentCulture = culture;
                
                // Set the current thread culture
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
                
                // In a real implementation, you might also want to:
                // 1. Trigger a UI refresh
                // 2. Store the preference
                // 3. Notify other components of the change
            }
            catch (CultureNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Culture not found: {languageCode}, error: {ex.Message}");
                // Fall back to English if the culture is not found
                _currentCulture = new CultureInfo("en");
            }
        }

        public string GetString(string key)
        {
            // In a real implementation, this would load from resource files
            // For now, return the key as a fallback
            // You would typically use something like:
            // return Resources.ResourceManager.GetString(key, _currentCulture) ?? key;
            
            return key;
        }
    }
}
