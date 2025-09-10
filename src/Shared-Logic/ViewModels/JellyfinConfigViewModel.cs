using Samsung_Jellyfin_Installer.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Samsung_Jellyfin_Installer.Shared.ViewModels
{
    public class JellyfinConfigViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        private string? _audioLanguagePreference;
        private string? _subtitleLanguagePreference;
        private string? _jellyfinServerIp;
        private string? _selectedTheme;
        private string? _selectedSubtitleMode;
        private string _jellyfinApiKey = string.Empty;
        private string _selectedUpdateMode = string.Empty;
        private string _selectedJellyfinPort = string.Empty;
        private bool _enableBackdrops;
        private bool _enableThemeSongs;
        private bool _enableThemeVideos;
        private bool _backdropScreensaver;
        private bool _detailsBanner;
        private bool _cinemaMode;
        private bool _nextUpEnabled;
        private bool _enableExternalVideoPlayers;
        private bool _skipIntros;
        private bool _autoPlayNextEpisode;
        private bool _rememberAudioSelections;
        private bool _rememberSubtitleSelections;
        private bool _playDefaultAudioTrack;
        private bool _userAutoLogin;
        private bool _apiKeyEnabled = false;
        private bool _apiKeySet = false;
        private ObservableCollection<JellyfinAuth> _availableJellyfinUsers = new();
        private JellyfinAuth? _selectedJellyfinUser;

        // TODO: Settings dependency needs to be abstracted for cross-platform support
        // Consider creating ISettingsService for platform-specific settings storage

        public string? AudioLanguagePreference
        {
            get => _audioLanguagePreference;
            set
            {
                if (SetProperty(ref _audioLanguagePreference, value))
                {
                    // Settings.Default.AudioLanguagePreference = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public string? SubtitleLanguagePreference
        {
            get => _subtitleLanguagePreference;
            set
            {
                if (SetProperty(ref _subtitleLanguagePreference, value))
                {
                    // Settings.Default.SubtitleLanguagePreference = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public string? JellyfinServerIp
        {
            get => _jellyfinServerIp;
            set
            {
                if (SetProperty(ref _jellyfinServerIp, value))
                {
                    UpdateJellyfinAddress();
                }
            }
        }

        public string? SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    // Settings.Default.SelectedTheme = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public string? SelectedSubtitleMode
        {
            get => _selectedSubtitleMode;
            set
            {
                if (SetProperty(ref _selectedSubtitleMode, value))
                {
                    // Settings.Default.SelectedSubtitleMode = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public string JellyfinApiKey
        {
            get => _jellyfinApiKey;
            set
            {
                if (SetProperty(ref _jellyfinApiKey, value))
                {
                    UpdateApiKeyStatus();
                    // Settings.Default.JellyfinApiKey = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                    _ = LoadJellyfinUsersAsync();
                }
            }
        }

        public string SelectedUpdateMode
        {
            get => _selectedUpdateMode;
            set => SetProperty(ref _selectedUpdateMode, value);
        }

        public string SelectedJellyfinPort
        {
            get => _selectedJellyfinPort;
            set
            {
                if (SetProperty(ref _selectedJellyfinPort, value))
                {
                    UpdateJellyfinAddress();
                }
            }
        }

        public bool EnableBackdrops
        {
            get => _enableBackdrops;
            set
            {
                if (SetProperty(ref _enableBackdrops, value))
                {
                    // Settings.Default.EnableBackdrops = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool EnableThemeSongs
        {
            get => _enableThemeSongs;
            set
            {
                if (SetProperty(ref _enableThemeSongs, value))
                {
                    // Settings.Default.EnableThemeSongs = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool EnableThemeVideos
        {
            get => _enableThemeVideos;
            set
            {
                if (SetProperty(ref _enableThemeVideos, value))
                {
                    // Settings.Default.EnableThemeVideos = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool BackdropScreensaver
        {
            get => _backdropScreensaver;
            set
            {
                if (SetProperty(ref _backdropScreensaver, value))
                {
                    // Settings.Default.BackdropScreensaver = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool DetailsBanner
        {
            get => _detailsBanner;
            set
            {
                if (SetProperty(ref _detailsBanner, value))
                {
                    // Settings.Default.DetailsBanner = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool CinemaMode
        {
            get => _cinemaMode;
            set
            {
                if (SetProperty(ref _cinemaMode, value))
                {
                    // Settings.Default.CinemaMode = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool NextUpEnabled
        {
            get => _nextUpEnabled;
            set
            {
                if (SetProperty(ref _nextUpEnabled, value))
                {
                    // Settings.Default.NextUpEnabled = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool EnableExternalVideoPlayers
        {
            get => _enableExternalVideoPlayers;
            set
            {
                if (SetProperty(ref _enableExternalVideoPlayers, value))
                {
                    // Settings.Default.EnableExternalVideoPlayers = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool SkipIntros
        {
            get => _skipIntros;
            set
            {
                if (SetProperty(ref _skipIntros, value))
                {
                    // Settings.Default.SkipIntros = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool AutoPlayNextEpisode
        {
            get => _autoPlayNextEpisode;
            set
            {
                if (SetProperty(ref _autoPlayNextEpisode, value))
                {
                    // Settings.Default.AutoPlayNextEpisode = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool RememberAudioSelections
        {
            get => _rememberAudioSelections;
            set
            {
                if (SetProperty(ref _rememberAudioSelections, value))
                {
                    // Settings.Default.RememberAudioSelections = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool RememberSubtitleSelections
        {
            get => _rememberSubtitleSelections;
            set
            {
                if (SetProperty(ref _rememberSubtitleSelections, value))
                {
                    // Settings.Default.RememberSubtitleSelections = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool PlayDefaultAudioTrack
        {
            get => _playDefaultAudioTrack;
            set
            {
                if (SetProperty(ref _playDefaultAudioTrack, value))
                {
                    // Settings.Default.PlayDefaultAudioTrack = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool UserAutoLogin
        {
            get => _userAutoLogin;
            set
            {
                if (SetProperty(ref _userAutoLogin, value))
                {
                    // Settings.Default.UserAutoLogin = value; // TODO: Abstract to ISettingsService
                    // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                }
            }
        }

        public bool ApiKeyEnabled
        {
            get => _apiKeyEnabled;
            set => SetProperty(ref _apiKeyEnabled, value);
        }

        public bool ApiKeySet
        {
            get => _apiKeySet;
            set => SetProperty(ref _apiKeySet, value);
        }

        public ObservableCollection<JellyfinAuth> AvailableJellyfinUsers
        {
            get => _availableJellyfinUsers;
            set => SetProperty(ref _availableJellyfinUsers, value);
        }

        public JellyfinAuth? SelectedJellyfinUser
        {
            get => _selectedJellyfinUser;
            set
            {
                if (SetProperty(ref _selectedJellyfinUser, value))
                {
                    // Save selected user ID
                    if (value != null)
                    {
                        // Settings.Default.JellyfinUserId = value.Id; // TODO: Abstract to ISettingsService
                        // Settings.Default.Save(); // TODO: Abstract to ISettingsService
                    }
                }
            }
        }

        // Static collections for UI binding
        public static ObservableCollection<string> AvailableThemes => new()
        {
            "appletv",
            "blueradiance", 
            "dark",
            "light",
            "purplehaze",
            "wmc"
        };

        public static ObservableCollection<string> AvailableSubtitleModes => new()
        {
            "None",
            "OnlyForced",
            "Default",
            "Always"
        };

        public static ObservableCollection<int> JellyfinPorts => new()
        {
            8096, 8920
        };

        public static ObservableCollection<string> AvailableUpdateModes => new()
        {
            "None",
            "Server Settings",
            "Browser Settings", 
            "User Settings",
            "Server & Browser Settings",
            "Server & User Settings",
            "Browser & User Settings",
            "All Settings"
        };

        public static ObservableCollection<string> AvailableLanguages => new()
        {
            "Abkhazian", "Afar", "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Assamese", "Azerbaijani",
            "Bashkir", "Basque", "Bengali", "Bihari", "Breton", "Bulgarian", "Burmese", "Catalan", "Chinese", "Croatian",
            "Czech", "Danish", "Dutch", "English", "Esperanto", "Estonian", "Finnish", "French", "Galician", "Georgian",
            "German", "Greek", "Hebrew", "Hindi", "Hungarian", "Icelandic", "Italian", "Japanese", "Korean", "Norwegian",
            "Polish", "Portuguese", "Romanian", "Russian", "Slovak", "Slovenian", "Spanish", "Swedish", "Turkish", "Ukrainian"
        };

        // Settings management
        public bool OtherDeviceSelected { get; set; }

        // Constructor
        public JellyfinConfigViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SamsungJellyfinInstaller/1.0");

            AvailableJellyfinUsers = new ObservableCollection<JellyfinAuth>();

            // Initialize with default values - TODO: Load from ISettingsService
            JellyfinServerIp = string.Empty;
            SelectedJellyfinPort = "8096"; // Default port
            SelectedUpdateMode = "None";
            JellyfinApiKey = string.Empty;
            SelectedTheme = "dark";
            SelectedSubtitleMode = "None";
            AudioLanguagePreference = string.Empty;
            SubtitleLanguagePreference = string.Empty;

            UpdateApiKeyStatus();
            _ = LoadJellyfinUsersAsync();
        }

        // Private methods
        private void UpdateJellyfinAddress()
        {
            if (!string.IsNullOrWhiteSpace(JellyfinServerIp) && !string.IsNullOrWhiteSpace(SelectedJellyfinPort))
            {
                // TODO: Save to ISettingsService - Settings.Default.JellyfinIP = $"{JellyfinServerIp}:{SelectedJellyfinPort}";
                Debug.WriteLine($"Updated Jellyfin IP: {JellyfinServerIp}:{SelectedJellyfinPort}");

                UpdateApiKeyStatus();
                _ = LoadJellyfinUsersAsync();
            }
        }

        private void UpdateApiKeyStatus()
        {
            var hasValidAddress = !string.IsNullOrEmpty(JellyfinServerIp) && !string.IsNullOrEmpty(SelectedJellyfinPort);
            var hasValidApiKey = !string.IsNullOrEmpty(JellyfinApiKey) && JellyfinApiKey.Length == 32;
            
            ApiKeySet = hasValidAddress && hasValidApiKey;
        }

        private bool IsValidJellyfinConfiguration()
        {
            var jellyfinAddress = $"{JellyfinServerIp}:{SelectedJellyfinPort}";
            return !string.IsNullOrEmpty(jellyfinAddress) &&
                   !string.IsNullOrEmpty(JellyfinApiKey) &&
                   JellyfinApiKey.Length == 32 &&
                   IsValidUrl($"http://{jellyfinAddress}/Users");
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async Task LoadJellyfinUsersAsync()
        {
            // Clear existing users first
            AvailableJellyfinUsers.Clear();

            if (!IsValidJellyfinConfiguration())
            {
                Debug.WriteLine("Invalid Jellyfin configuration - skipping user load");
                return;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SamsungJellyfinInstaller/1.0");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"MediaBrowser Token=\"{JellyfinApiKey}\"");

                var jellyfinAddress = $"{JellyfinServerIp}:{SelectedJellyfinPort}";
                var url = $"http://{jellyfinAddress}/Users";
                Debug.WriteLine($"Attempting to load users from: {url}");

                using var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<JellyfinAuth>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (users != null && users.Any())
                    {
                        foreach (var user in users)
                        {
                            AvailableJellyfinUsers.Add(user);
                        }

                        if (AvailableJellyfinUsers.Count > 1)
                        {
                            AvailableJellyfinUsers.Add(new JellyfinAuth
                            {
                                Id = "everyone",
                                Name = "Everyone"
                            });
                        }

                        // TODO: Restore previously selected user from ISettingsService
                        // var savedUserId = Settings.Default.JellyfinUserId;
                        // if (!string.IsNullOrEmpty(savedUserId))
                        // {
                        //     SelectedJellyfinUser = AvailableJellyfinUsers.FirstOrDefault(u => u.Id == savedUserId);
                        // }

                        // If no user selected and only one user available, select it
                        if (SelectedJellyfinUser == null && AvailableJellyfinUsers.Count == 1)
                        {
                            SelectedJellyfinUser = AvailableJellyfinUsers.First();
                        }

                        Debug.WriteLine($"Successfully loaded {AvailableJellyfinUsers.Count} users");
                    }
                    else
                    {
                        Debug.WriteLine("No users found in response");
                    }
                }
                else
                {
                    Debug.WriteLine($"Failed to load users - Status: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP error loading Jellyfin users: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Request timeout loading Jellyfin users: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error loading Jellyfin users: {ex.Message}");
            }
        }

        // Public methods for settings management
        public void LoadSettingsFromStorage()
        {
            // TODO: Implement when ISettingsService is available
            // This method should load all settings from the platform-specific storage
        }

        public void SaveSettingsToStorage()
        {
            // TODO: Implement when ISettingsService is available
            // This method should save all settings to the platform-specific storage
        }
    }
}
