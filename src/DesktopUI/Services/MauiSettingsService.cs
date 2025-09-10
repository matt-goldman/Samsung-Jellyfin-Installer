using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiSettingsService : ISettingsService
    {
        public T? GetSetting<T>(string key)
        {
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                // Use reflection to call the appropriate Preferences.Get overload
                var getMethod = typeof(Preferences).GetMethods()
                    .FirstOrDefault(m => m.Name == "Get" &&
                                   m.GetParameters().Length == 2 &&
                                   m.GetParameters()[1].ParameterType == underlyingType);

                if (getMethod != null)
                {
                    var defaultValue = underlyingType == typeof(string) ? string.Empty :
                                     underlyingType == typeof(bool) ? false :
                                     underlyingType == typeof(int) ? 0 :
                                     underlyingType == typeof(double) ? 0.0 :
                                     underlyingType == typeof(float) ? 0.0f :
                                     underlyingType == typeof(long) ? 0L :
                                     underlyingType == typeof(DateTime) ? DateTime.MinValue :
                                     Activator.CreateInstance(underlyingType);

                    var result = getMethod.Invoke(null, new object[] { key, defaultValue! });
                    return (T?)result;
                }
            }
            catch
            {
                // Fallback handled below
            }

            // Fallback for unsupported types
            return default(T);
        }

        public void SetSetting<T>(string key, T value)
        {
            if (value == null)
            {
                Preferences.Remove(key);
                return;
            }

            var type = value.GetType();
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                // Use reflection to call the appropriate Preferences.Set overload
                var setMethod = typeof(Preferences).GetMethod("Set", new[] { typeof(string), underlyingType });
                if (setMethod != null)
                {
                    setMethod.Invoke(null, new object[] { key, value });
                }
                else
                {
                    // Fallback to string representation for unsupported types
                    Preferences.Set(key, value.ToString() ?? string.Empty);
                }
            }
            catch
            {
                // Fallback to string representation if reflection fails
                Preferences.Set(key, value.ToString() ?? string.Empty);
            }
        }

        public void Save()
        {
            // MAUI Preferences are automatically saved, no explicit save needed
        }
    }
}
