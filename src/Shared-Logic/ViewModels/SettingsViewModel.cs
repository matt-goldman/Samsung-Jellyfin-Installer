using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Samsung_Jellyfin_Installer.Shared.Models;
using Samsung_Jellyfin_Installer.Shared.Services;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Samsung_Jellyfin_Installer.Shared.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private readonly IFileDialogService _fileDialogService;
        private readonly ITizenInstallerService _tizenInstallerService;
        private readonly ILocalizationService _localizationService;

        private LanguageOption? _selectedLanguage;
        private ExistingCertificates? _selectedCertificateObject;
        private string _selectedCertificate = string.Empty;
        private string _customWgtPath = string.Empty;
        private bool _rememberCustomIP;
        private bool _deletePreviousInstall;
        private bool _forceSamsungLogin;
        private bool _rtlReading;

        public LanguageOption? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value) && value != null)
                {
                    _localizationService.ChangeLanguage(value.Code);
                    _settingsService.SetSetting("Language", value.Code);
                }
            }
        }

        public ExistingCertificates? SelectedCertificateObject
        {
            get => _selectedCertificateObject;
            set
            {
                if (SetProperty(ref _selectedCertificateObject, value))
                {
                    SelectedCertificate = value?.Name ?? string.Empty;
                }
            }
        }

        public string SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                if (SetProperty(ref _selectedCertificate, value))
                {
                    _settingsService.SetSetting("SelectedCertificate", value);
                }
            }
        }

        public string CustomWgtPath
        {
            get => _customWgtPath;
            set
            {
                if (SetProperty(ref _customWgtPath, value))
                {
                    _settingsService.SetSetting("CustomWgtPath", value);
                }
            }
        }

        public bool RememberCustomIP
        {
            get => _rememberCustomIP;
            set
            {
                if (SetProperty(ref _rememberCustomIP, value))
                {
                    _settingsService.SetSetting("RememberCustomIP", value);
                }
            }
        }

        public bool DeletePreviousInstall
        {
            get => _deletePreviousInstall;
            set
            {
                if (SetProperty(ref _deletePreviousInstall, value))
                {
                    _settingsService.SetSetting("DeletePreviousInstall", value);
                }
            }
        }

        public bool ForceSamsungLogin
        {
            get => _forceSamsungLogin;
            set
            {
                if (SetProperty(ref _forceSamsungLogin, value))
                {
                    _settingsService.SetSetting("ForceSamsungLogin", value);
                }
            }
        }

        public bool RTLReading
        {
            get => _rtlReading;
            set
            {
                if (SetProperty(ref _rtlReading, value))
                {
                    _settingsService.SetSetting("RTLReading", value);
                }
            }
        }

        // Collections
        public ObservableCollection<LanguageOption> AvailableLanguages { get; } = new();
        public ObservableCollection<ExistingCertificates> AvailableCertificates { get; } = new();

        // Commands
        public IRelayCommand ModifyConfigCommand { get; }
        public IRelayCommand BrowseWgtCommand { get; }

        public SettingsViewModel(
            ISettingsService settingsService,
            IFileDialogService fileDialogService,
            ITizenInstallerService tizenInstallerService,
            ILocalizationService localizationService)
        {
            _settingsService = settingsService;
            _fileDialogService = fileDialogService;
            _tizenInstallerService = tizenInstallerService;
            _localizationService = localizationService;

            // Initialize commands
            ModifyConfigCommand = new RelayCommand(ExecuteModifyConfig);
            BrowseWgtCommand = new RelayCommand(async () => await BrowseWgtFileAsync());

            // Initialize data
            InitializeLanguages();
            _ = Task.Run(InitializeCertificatesAsync);
            LoadSettings();
        }

        private void InitializeLanguages()
        {
            // Manually define supported languages
            var supportedLanguages = new List<LanguageOption>
            {
                new() { Code = "en", Name = "English" },
                new() { Code = "da", Name = "Dansk" },
                new() { Code = "nl", Name = "Nederlands" }
            };

            // TODO: Verify resources actually exist for cross-platform scenarios
            // For now, add all supported languages
            foreach (var language in supportedLanguages)
            {
                AvailableLanguages.Add(language);
            }
        }

        private async Task InitializeCertificatesAsync()
        {
            try
            {
                // TODO: This will need platform-specific implementation
                // For now, add a placeholder
                AvailableCertificates.Add(new ExistingCertificates
                {
                    Name = "Default Certificate"
                });
            }
            catch (Exception ex)
            {
                // TODO: Log error
                System.Diagnostics.Debug.WriteLine($"Error initializing certificates: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            var savedLangCode = _settingsService.GetSetting<string>("Language") ?? "en";
            SelectedLanguage = AvailableLanguages.FirstOrDefault(lang => lang.Code == savedLangCode)
                              ?? AvailableLanguages.FirstOrDefault(lang => lang.Code == "en");

            CustomWgtPath = _settingsService.GetSetting<string>("CustomWgtPath") ?? string.Empty;
            RememberCustomIP = _settingsService.GetSetting<bool>("RememberCustomIP");
            DeletePreviousInstall = _settingsService.GetSetting<bool>("DeletePreviousInstall");
            ForceSamsungLogin = _settingsService.GetSetting<bool>("ForceSamsungLogin");
            RTLReading = _settingsService.GetSetting<bool>("RTLReading");
        }

        private void ExecuteModifyConfig()
        {
            // TODO: This needs to be abstracted for cross-platform navigation
            // For now, this is a placeholder that would be implemented
            // differently in each UI platform
        }

        private async Task BrowseWgtFileAsync()
        {
            try
            {
                var filter = "Widget files (*.wgt)|*.wgt|All files (*.*)|*.*";
                var selectedFile = await _fileDialogService.OpenFileAsync("Select Widget File", filter);
                
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    CustomWgtPath = selectedFile;
                }
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error browsing for WGT file: {ex.Message}");
            }
        }
    }
}
