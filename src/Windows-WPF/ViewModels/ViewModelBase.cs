// This ViewModelBase is obsolete - inherit directly from ObservableObject
// Update ViewModels to inherit from CommunityToolkit.Mvvm.ComponentModel.ObservableObject directly

using CommunityToolkit.Mvvm.ComponentModel;

namespace Samsung_Jellyfin_Installer.ViewModels
{
    // Alias for backward compatibility during migration
    public class ViewModelBase : ObservableObject
    {
        // This class serves as a bridge during the migration to ObservableObject
        // All WPF ViewModels should eventually inherit directly from ObservableObject
    }
}
