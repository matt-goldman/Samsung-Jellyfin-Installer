using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Samsung_Jellyfin_Installer.Shared.Models;
using Samsung_Jellyfin_Installer.Shared.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Samsung_Jellyfin_Installer.Shared.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ITizenInstallerService _tizenInstaller;
        private readonly IDialogService _dialogService;
        private readonly INetworkService _networkService;
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;

        private ObservableCollection<GitHubRelease> _releases = new();
        private ObservableCollection<Asset> _availableAssets = new();
        private ObservableCollection<NetworkDevice> _availableDevices = new();
        private GitHubRelease? _selectedRelease;
        private Asset? _selectedAsset;
        private NetworkDevice? _selectedDevice;
        private JellyfinConfigViewModel? _jellyfinConfigViewModel;
        private bool _isLoading;
        private bool _isLoadingDevices;
        private string _statusBar = string.Empty;
        private string _downloadedPackagePath = string.Empty;

        public ObservableCollection<GitHubRelease> Releases
        {
            get => _releases;
            private set => SetProperty(ref _releases, value);
        }

        public ObservableCollection<Asset> AvailableAssets
        {
            get => _availableAssets;
            private set => SetProperty(ref _availableAssets, value);
        }

        public ObservableCollection<NetworkDevice> AvailableDevices
        {
            get => _availableDevices;
            private set => SetProperty(ref _availableDevices, value);
        }

        public GitHubRelease? SelectedRelease
        {
            get => _selectedRelease;
            set
            {
                if (SetProperty(ref _selectedRelease, value))
                {
                    RefreshAssets();
                }
            }
        }

        public Asset? SelectedAsset
        {
            get => _selectedAsset;
            set => SetProperty(ref _selectedAsset, value);
        }

        public NetworkDevice? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetProperty(ref _selectedDevice, value) && value?.IpAddress == "Other")
                {
                    if (_jellyfinConfigViewModel != null)
                        _jellyfinConfigViewModel.OtherDeviceSelected = true;
                }
            }
        }

        public JellyfinConfigViewModel? JellyfinConfigViewModel
        {
            get => _jellyfinConfigViewModel;
            set => SetProperty(ref _jellyfinConfigViewModel, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(IsControlsEnabled));
                    // Notify that command can execute states may have changed
                    RefreshAllRelayCommands();
                }
            }
        }

        public bool IsLoadingDevices
        {
            get => _isLoadingDevices;
            set
            {
                if (SetProperty(ref _isLoadingDevices, value))
                {
                    OnPropertyChanged(nameof(IsControlsEnabled));
                    OnPropertyChanged(nameof(EnableDevicesInput));
                }
            }
        }

        public string StatusBar
        {
            get => _statusBar;
            set => SetProperty(ref _statusBar, value);
        }

        public string DownloadedPackagePath
        {
            get => _downloadedPackagePath;
            set => SetProperty(ref _downloadedPackagePath, value);
        }

        // Computed properties
        public bool IsControlsEnabled => !IsLoading && !IsLoadingDevices;
        public bool EnableDevicesInput => !IsLoadingDevices;

        // Commands - using MVVM Community Toolkit
        public IRelayCommand LoadReleasesCommand { get; }
        public IRelayCommand ScanDevicesCommand { get; }
        public IRelayCommand InstallCommand { get; }
        public IRelayCommand StopSdbServerCommand { get; }

        // Constructor
        public MainWindowViewModel(
            ITizenInstallerService tizenInstaller,
            IDialogService dialogService,
            INetworkService networkService,
            IServiceProvider serviceProvider,
            HttpClient httpClient)
        {
            _tizenInstaller = tizenInstaller;
            _dialogService = dialogService;
            _networkService = networkService;
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;

            // Initialize commands
            LoadReleasesCommand = new RelayCommand(async () => await LoadReleasesAsync(), () => !IsLoading);
            ScanDevicesCommand = new RelayCommand(async () => await ScanDevicesAsync(), () => !IsLoadingDevices);
            InstallCommand = new RelayCommand(async () => await InstallAsync(), CanInstall);
            StopSdbServerCommand = new RelayCommand(async () => await StopSdbServerAsync());

            // Initialize collections
            Releases = new ObservableCollection<GitHubRelease>();
            AvailableAssets = new ObservableCollection<Asset>();
            AvailableDevices = new ObservableCollection<NetworkDevice>();

            // Load initial data
            _ = Task.Run(LoadReleasesAsync);
        }

        // Command implementations
        private async Task LoadReleasesAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            StatusBar = "Loading releases..."; // TODO: Localize

            try
            {
                var url = "https://api.github.com/repos/jellyfin/jellyfin-tizen/releases";
                using var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var releases = System.Text.Json.JsonSerializer.Deserialize<List<GitHubRelease>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (releases != null)
                {
                    Releases.Clear();
                    foreach (var release in releases)
                    {
                        if (release.Assets?.Any() == true)
                        {
                            Releases.Add(release);
                        }
                    }

                    SelectedRelease = Releases.FirstOrDefault();
                }

                StatusBar = $"Loaded {Releases.Count} releases"; // TODO: Localize
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading releases: {ex.Message}");
                await _dialogService.ShowErrorAsync($"Failed to load releases: {ex.Message}");
                StatusBar = "Failed to load releases"; // TODO: Localize
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ScanDevicesAsync()
        {
            if (IsLoadingDevices) return;

            IsLoadingDevices = true;
            StatusBar = "Scanning for devices..."; // TODO: Localize

            try
            {
                AvailableDevices.Clear();

                var devices = await _networkService.GetLocalTizenAddresses();
                var deviceList = devices.ToList();
                foreach (var device in deviceList)
                {
                    AvailableDevices.Add(device);
                }

                // Add "Other" option
                AvailableDevices.Add(new NetworkDevice
                {
                    IpAddress = "Other",
                });

                StatusBar = $"Found {deviceList.Count} devices"; // TODO: Localize
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error scanning for devices: {ex.Message}");
                await _dialogService.ShowErrorAsync($"Failed to scan for devices: {ex.Message}");
                StatusBar = "Failed to scan for devices"; // TODO: Localize
            }
            finally
            {
                IsLoadingDevices = false;
            }
        }

        private async Task InstallAsync()
        {
            if (!CanInstall() || SelectedAsset == null || SelectedDevice == null) return;

            IsLoading = true;
            StatusBar = "Starting installation..."; // TODO: Localize

            try
            {
                // TODO: Implement installation logic
                // This will require careful abstraction of UI-specific operations
                await _dialogService.ShowMessageAsync("Installation feature needs implementation in shared logic");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during installation: {ex.Message}");
                await _dialogService.ShowErrorAsync($"Installation failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                StatusBar = "Ready"; // TODO: Localize
            }
        }

        private async Task StopSdbServerAsync()
        {
            try
            {
                // TODO: Abstract this functionality
                StatusBar = "Stopping SDB server..."; // TODO: Localize
                
                await _dialogService.ShowMessageAsync("SDB server stop functionality needs implementation");
                
                StatusBar = "SDB server stopped"; // TODO: Localize
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to stop SDB server: {ex.Message}");
                await _dialogService.ShowErrorAsync($"Failed to stop SDB server: {ex.Message}");
            }
        }

        // Helper methods
        private bool CanInstall()
        {
            return !IsLoading && SelectedAsset != null && SelectedDevice != null;
        }

        private void RefreshAssets()
        {
            AvailableAssets.Clear();
            
            if (SelectedRelease?.Assets != null)
            {
                foreach (var asset in SelectedRelease.Assets)
                {
                    AvailableAssets.Add(asset);
                }
                
                SelectedAsset = AvailableAssets.FirstOrDefault();
            }
        }

        private void RefreshAllRelayCommands()
        {
            // MVVM Community Toolkit handles this automatically in most cases
            // But we can manually trigger if needed
            LoadReleasesCommand.NotifyCanExecuteChanged();
            ScanDevicesCommand.NotifyCanExecuteChanged();
            InstallCommand.NotifyCanExecuteChanged();
        }

        // TODO: These methods need to be abstracted for cross-platform support
        // They currently contain WPF-specific logic that needs to be moved to platform-specific implementations

        public async Task<NetworkDevice?> GetDeveloperInfoAsync(NetworkDevice device)
        {
            // TODO: Abstract this functionality
            await _dialogService.ShowMessageAsync("GetDeveloperInfoAsync needs implementation");
            return device;
        }

        public void SetJellyfinConfigViewModel(JellyfinConfigViewModel viewModel)
        {
            JellyfinConfigViewModel = viewModel;
        }
    }
}
