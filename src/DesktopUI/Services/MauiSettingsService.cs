using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiSettingsService : ISettingsService
    {
        public T? GetSetting<T>(string key)
        {
            if (typeof(T) == typeof(string))
            {
                var value = Preferences.Get(key, string.Empty);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(bool))
            {
                var value = Preferences.Get(key, false);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(int))
            {
                var value = Preferences.Get(key, 0);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(double))
            {
                var value = Preferences.Get(key, 0.0);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(float))
            {
                var value = Preferences.Get(key, 0.0f);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(long))
            {
                var value = Preferences.Get(key, 0L);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(DateTime))
            {
                var value = Preferences.Get(key, DateTime.MinValue);
                return (T)(object)value;
            }

            // For complex types, we could use JSON serialization
            // For now, return default value
            return default(T);
        }

        public void SetSetting<T>(string key, T value)
        {
            if (value is string stringValue)
            {
                Preferences.Set(key, stringValue);
            }
            else if (value is bool boolValue)
            {
                Preferences.Set(key, boolValue);
            }
            else if (value is int intValue)
            {
                Preferences.Set(key, intValue);
            }
            else if (value is double doubleValue)
            {
                Preferences.Set(key, doubleValue);
            }
            else if (value is float floatValue)
            {
                Preferences.Set(key, floatValue);
            }
            else if (value is long longValue)
            {
                Preferences.Set(key, longValue);
            }
            else if (value is DateTime dateTimeValue)
            {
                Preferences.Set(key, dateTimeValue);
            }
            else
            {
                // For complex types, we could use JSON serialization
                // For now, convert to string representation
                Preferences.Set(key, value?.ToString() ?? string.Empty);
            }
        }

        public void Save()
        {
            // MAUI Preferences are automatically saved, no explicit save needed
        }
    }
}
