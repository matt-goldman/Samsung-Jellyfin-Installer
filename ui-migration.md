# UI Control Migration Guide: WPF to .NET MAUI

This document maps WPF controls used in the Samsung Jellyfin Installer to their .NET MAUI equivalents to facilitate UI migration.

| WPF Control | .NET MAUI Equivalent | Files Used In |
|-------------|---------------------|---------------|
| **Window** | ContentPage | `MainWindow.xaml`, `SettingsView.xaml`, `SamsungLoginWindow.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **Grid** | Grid | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `SamsungLoginWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **StackPanel** | VerticalStackLayout | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **StackPanel Orientation="Horizontal"** | HorizontalStackLayout | `IpInputDialog.xaml`, `InstallationCompleteWindow.xaml` |
| **Button** | Button | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallationCompleteWindow.xaml` |
| **TextBlock** | Label | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `SamsungLoginWindow.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **TextBox** | Entry | `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml` |
| **ComboBox** | Picker | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **CheckBox** | CheckBox | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **Label** | Label | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **Border** | Border | `MainWindow.xaml` |
| **ProgressBar** | ProgressBar | `InstallingWindow.xaml` |
| **ScrollViewer** | ScrollView | `JellyfinConfigView.xaml` |
| **Hyperlink** | Unknown | `MainWindow.xaml` |
| **Run** | Unknown | `MainWindow.xaml` |
| **RowDefinition** | RowDefinition | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |
| **ColumnDefinition** | ColumnDefinition | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |

## Material Design Controls (3rd Party - Need Custom Implementation)

| WPF Control | .NET MAUI Equivalent | Notes | Files Used In |
|-------------|---------------------|-------|---------------|
| **materialDesign:Card** | Custom Card Control | Need to create custom card control or use Frame with styling | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **materialDesign:PackIcon** | Custom Icon Control | Need to create custom icon control using fonts or images | `MainWindow.xaml`, `SettingsView.xaml` |
| **materialDesign:MaterialDesignFont** | Custom Font | Need to include Material Design Icons font | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |
| **materialDesign:TextFieldAssist.UnderlineBrush** | Custom Styling | Need custom styling for Entry controls | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **materialDesign:HintAssist.Hint** | Entry.Placeholder | Use built-in Placeholder property | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **materialDesign:CustomColorTheme** | Custom Theme | Need to implement custom theme in MAUI | `App.xaml` |

## WebView Controls (3rd Party)

| WPF Control | .NET MAUI Equivalent | Notes | Files Used In |
|-------------|---------------------|-------|---------------|
| **wv2:WebView2** | WebAuthenticator | Use Microsoft.Maui.Authentication.WebAuthenticator for OAuth flows. For Windows, consider WinUIEx.WebAuthenticator for enhanced functionality. This is preferred over WebView for authentication as it uses the system browser and provides better security. | `SamsungLoginWindow.xaml` |

## Authentication Implementation Notes

### Samsung Login Migration Strategy

**Current WPF Implementation:**

- Uses WebView2 with embedded browser
- Requires custom HTTP callback server (`SamsungLoginService.StartCallbackServer()`)
- Manual window management and state handling

**Recommended MAUI Implementation:**

- **Cross-Platform:** `Microsoft.Maui.Authentication.WebAuthenticator`
- **Windows Enhanced:** `WinUIEx.WebAuthenticator` for additional Windows-specific features
- **macOS/iOS:** Native `SFSafariViewController`/`ASWebAuthenticationSession`
- **Android:** Custom Tabs with native browser integration

**Benefits of WebAuthenticator:**

1. **Security:** Uses system browser instead of embedded WebView
2. **No HTTP Server:** Built-in URL callback handling
3. **Platform Native:** Leverages platform-specific secure authentication
4. **Simpler Code:** Eliminates custom server and window management
5. **Better UX:** Users can use saved passwords/biometrics from system browser

**Implementation Example:**

```csharp
public async Task<SamsungAuth> AuthenticateAsync()
{
    var authUrl = "https://account.samsung.com/accounts/be1dce529476c1a6d407c4c7578c31bd/signInGate...";
    var callbackUrl = "samsungjellyfininstaller://authenticated";
    
    var result = await WebAuthenticator.AuthenticateAsync(
        new WebAuthenticatorOptions
        {
            Url = new Uri(authUrl),
            CallbackUrl = new Uri(callbackUrl)
        });
    
    return ParseSamsungAuthResult(result);
}
```

**Platform Configuration Required:**

- **iOS:** URL scheme in Info.plist
- **Android:** Intent filter in AndroidManifest.xml  
- **Windows:** Protocol registration
- **macOS:** URL scheme in Info.plist

## Dynamic Resources & Styles

| WPF Style | .NET MAUI Equivalent | Notes | Files Used In |
|-----------|---------------------|-------|---------------|
| **MaterialDesignTitleTextBlock** | Custom Style | Need to create custom Label style | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignSubtitle1TextBlock** | Custom Style | Need to create custom Label style | `JellyfinConfigView.xaml` |
| **MaterialDesignComboBox** | Custom Style | Need to create custom Picker style | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignTextBox** | Custom Style | Need to create custom Entry style | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignCheckBox** | Custom Style | Need to create custom CheckBox style | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignRaisedLightButton** | Custom Style | Need to create custom Button style | `MainWindow.xaml`, `SettingsView.xaml`, `InstallationCompleteWindow.xaml` |
| **MaterialDesignFloatingActionLightButton** | Custom Style | Need to create custom floating action Button style | `MainWindow.xaml` |
| **MaterialDesignToolButton** | Custom Style | Need to create custom tool Button style | `SettingsView.xaml` |

## Migration Priority by File

### High Priority (Core Functionality)

1. **MainWindow.xaml** - Main application interface
2. **SettingsView.xaml** - Settings configuration
3. **JellyfinConfigView.xaml** - Jellyfin configuration (longest/most complex)

### Medium Priority (Installation Flow)

1. **InstallingWindow.xaml** - Installation progress
2. **InstallationCompleteWindow.xaml** - Installation completion

### Low Priority (Dialogs)

1. **IpInputDialog.xaml** - Simple IP input dialog
2. **SamsungLoginWindow.xaml** - Samsung login (WebView)

## Implementation Notes

1. **Material Design**: The app heavily uses Material Design controls. Consider using a Material Design library for MAUI or implementing custom controls.

2. **Card Control**: The `materialDesign:Card` is extensively used. A custom Card control or styled Frame will be essential.

3. **Icons**: `materialDesign:PackIcon` controls need replacement with font icons or image-based icons.

4. **Styling**: Heavy use of Material Design styles requires comprehensive custom styling in MAUI.

5. **WebView**: The Samsung login window uses WebView2, which has a direct MAUI equivalent.

6. **Complex Layouts**: `JellyfinConfigView.xaml` has the most complex layout with nested grids and many controls.

7. **Resource Dictionaries**: App.xaml references Material Design themes that need custom MAUI implementation.
