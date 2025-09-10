# UI Control Migration Guide: WPF to .NET MAUI

This document maps WPF controls used in the Samsung Jellyfin Installer to their .NET MAUI equivalents to facilitate UI migration.

## Basic UI Controls

| WPF Control | .NET MAUI Equivalent | Status | Files Used In |
|-------------|---------------------|--------|---------------|
| **Window** | ContentPage | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `SamsungLoginWindow.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **Grid** | Grid | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `SamsungLoginWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **StackPanel** | VerticalStackLayout | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **StackPanel Orientation="Horizontal"** | HorizontalStackLayout | ✅ Complete | `IpInputDialog.xaml`, `InstallationCompleteWindow.xaml` |
| **Button** | Button | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `InstallationCompleteWindow.xaml` |
| **TextBlock** | Label | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml`, `SamsungLoginWindow.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **TextBox** | Entry | ✅ Complete | `SettingsView.xaml`, `JellyfinConfigView.xaml`, `IpInputDialog.xaml` |
| **ComboBox** | Picker | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **CheckBox** | CheckBox | ✅ Complete | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **Label** | Label | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **Border** | Border | ✅ Complete | `MainWindow.xaml` |
| **ProgressBar** | ProgressBar | ✅ Complete | `InstallingWindow.xaml` |
| **ScrollViewer** | ScrollView | ✅ Complete | `JellyfinConfigView.xaml` |
| **Hyperlink** | Label + TapGestureRecognizer | ✅ Complete | `MainWindow.xaml` |
| **Run** | Span (in FormattedString) | ✅ Complete | `MainWindow.xaml` |
| **RowDefinition** | RowDefinition | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |
| **ColumnDefinition** | ColumnDefinition | ✅ Complete | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |

## Material Design Controls (3rd Party)

| WPF Control | .NET MAUI Equivalent | Status | Notes | Files Used In |
|-------------|---------------------|--------|-------|---------------|
| **materialDesign:Card** | Frame (placeholder) | 🔧 In Progress | Currently using Frame as placeholder, needs custom Card control or better styling | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallingWindow.xaml`, `InstallationCompleteWindow.xaml` |
| **materialDesign:PackIcon** | Unicode Icons/Labels | 🔧 In Progress | Currently using Unicode icons (🔄, ⚙️), needs proper icon font implementation | `MainWindow.xaml`, `SettingsView.xaml` |
| **materialDesign:MaterialDesignFont** | Custom Font | ❌ Not Started | Need to include Material Design Icons font | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml`, `InstallationCompleteWindow.xaml` |
| **materialDesign:TextFieldAssist.UnderlineBrush** | Custom Styling | ❌ Not Started | Need custom styling for Entry controls | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **materialDesign:HintAssist.Hint** | Entry.Placeholder | ✅ Complete | Using built-in Placeholder property | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **materialDesign:CustomColorTheme** | Custom Theme | ❌ Not Started | Need to implement custom theme in MAUI | `App.xaml` |

## WebView Controls (3rd Party)

| WPF Control | .NET MAUI Equivalent | Status | Notes | Files Used In |
|-------------|---------------------|--------|-------|---------------|
| **wv2:WebView2** | WebAuthenticator | 🔧 In Progress | Placeholder implementation created, needs WebAuthenticator integration | `SamsungLoginWindow.xaml` |

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

| WPF Style | .NET MAUI Equivalent | Status | Notes | Files Used In |
|-----------|---------------------|--------|-------|---------------|
| **MaterialDesignTitleTextBlock** | Custom Style | ❌ Not Started | Need to create custom Label style | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignSubtitle1TextBlock** | Custom Style | ❌ Not Started | Need to create custom Label style | `JellyfinConfigView.xaml` |
| **MaterialDesignComboBox** | Custom Style | ❌ Not Started | Need to create custom Picker style | `MainWindow.xaml`, `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignTextBox** | Custom Style | ❌ Not Started | Need to create custom Entry style | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignCheckBox** | Custom Style | ❌ Not Started | Need to create custom CheckBox style | `SettingsView.xaml`, `JellyfinConfigView.xaml` |
| **MaterialDesignRaisedLightButton** | Custom Style | ❌ Not Started | Need to create custom Button style | `MainWindow.xaml`, `SettingsView.xaml`, `InstallationCompleteWindow.xaml` |
| **MaterialDesignFloatingActionLightButton** | Custom Style | ❌ Not Started | Need to create custom floating action Button style | `MainWindow.xaml` |
| **MaterialDesignToolButton** | Custom Style | ❌ Not Started | Need to create custom tool Button style | `SettingsView.xaml` |

## Views Migration Status

### ✅ Complete (Core Views)

1. **IpInputDialog.xaml** - Simple IP input dialog ✅ **Complete**
2. **InstallationCompleteWindow.xaml** - Installation completion ✅ **Complete**
3. **InstallingWindow.xaml** - Installation progress ✅ **Complete**
4. **MainWindow.xaml** - Main application interface ✅ **Complete**
5. **SettingsView.xaml** - Settings configuration ✅ **Complete**
6. **JellyfinConfigView.xaml** - Jellyfin configuration (simplified) ✅ **Complete**

### 🔧 In Progress

1. **SamsungLoginWindow.xaml** - Samsung login 🔧 **In Progress** (Placeholder created, needs WebAuthenticator)

## Build Status

✅ **All Views Build Successfully**
- Windows (net9.0-windows10.0.19041.0): ✅ Success
- macOS (net9.0-maccatalyst): ✅ Success
- Only warnings about data binding performance optimizations (not errors)

## Next Steps - UI Migration Only

### 🔧 High Priority

#### 1. Material Design Styling System
- **Create custom Card control** to replace Frame placeholders
- **Implement Material Design color scheme** (#FF000B25 primary color maintained)
- **Create consistent button styles** (Raised, Floating Action, Tool buttons)
- **Design Entry/Picker styling** to match Material Design aesthetics

#### 2. Icon System Implementation
- **Replace Unicode placeholders** (🔄, ⚙️) with proper Material Design icons
- **Add Material Design Icons font** to the project
- **Create icon mapping system** for materialDesign:PackIcon equivalents
- **Implement folder/browse icons** for file selection buttons

#### 3. Complete JellyfinConfigView
- **Expand the simplified sections** (currently has placeholder for complex settings)
- **Add remaining configuration controls** from the original 341-line WPF version
- **Implement proper section organization** with collapsible/expandable areas
- **Add validation and input formatting** for complex configuration fields

### 🔧 Medium Priority

#### 4. Samsung Login Implementation
- **Integrate WebAuthenticator** for cross-platform authentication
- **Configure platform-specific URL schemes** (iOS Info.plist, Android manifest, Windows protocol)
- **Remove placeholder implementation** and add real authentication flow
- **Test authentication flow** on each target platform

#### 5. Enhanced Visual Polish
- **Add proper shadows and elevation** to replace Material Design Card effects
- **Implement consistent spacing and margins** across all views
- **Add loading states and transitions** for better UX
- **Create responsive layouts** for different window sizes

#### 6. Accessibility and Localization Prep
- **Add proper accessibility labels** for all interactive elements
- **Prepare for localization** by extracting hardcoded strings
- **Test keyboard navigation** and screen reader compatibility
- **Implement proper focus management** between controls

### 🔧 Low Priority (Polish)

#### 7. Advanced Layout Features
- **Add proper window sizing constraints** (MinWidth, MinHeight equivalent)
- **Implement window state management** for desktop platforms
- **Add context menus** where appropriate
- **Create consistent error handling UI** patterns

#### 8. Performance Optimizations
- **Add x:DataType declarations** to eliminate binding warnings
- **Implement compiled bindings** for better performance
- **Optimize complex layouts** with virtualization where needed
- **Add lazy loading** for heavy configuration sections

### ❌ Not In Scope (UI Migration)

The following items are **NOT** part of UI migration and should be handled separately:
- ViewModel integration and data binding setup
- Service injection and dependency configuration  
- Business logic implementation
- Platform-specific service implementations
- Navigation and routing setup
- Data persistence and settings management
- Network operations and API calls
- Installation and deployment logic

## Implementation Notes

1. **Material Design**: The app heavily relies on Material Design. Consider using a Material Design library for MAUI or implementing custom controls with the established color scheme (#FF000B25).

2. **Frame as Card Placeholder**: All `materialDesign:Card` instances are currently using `Frame` with basic styling. This provides the correct layout but needs visual enhancement.

3. **Layout Preservation**: Grid layouts, row/column definitions, and spacing have been preserved from the original WPF implementation.

4. **Binding Compatibility**: All data binding expressions have been maintained to ensure compatibility with existing ViewModels.

5. **Cross-Platform Considerations**: Views are designed to work on both Windows and macOS target platforms.

6. **Build Warnings**: Current XamlC warnings about compiled bindings can be resolved by adding x:DataType declarations to improve performance.
