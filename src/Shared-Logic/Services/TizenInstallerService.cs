// TODO: Technical Debt - This service contains UI dependencies that need to be refactored
// All MessageBox.Show calls should be replaced with IDialogService calls
// Views imports should be removed
// This service violates separation of concerns by directly showing UI elements
//
// Actions needed:
// 1. Inject IDialogService and replace all MessageBox.Show calls
// 2. Remove Views dependency 
// 3. Move UI interaction logic to ViewModels
// 4. Create abstraction for file dialogs if needed

using Samsung_Jellyfin_Installer.Shared.Models;

namespace Samsung_Jellyfin_Installer.Shared.Services
{
    // Placeholder - TizenInstallerService moved here as-is but needs refactoring
    // Original implementation has WPF dependencies that violate MVVM pattern
    public interface ITizenInstallerServiceRefactorNeeded
    {
        // This interface represents the original service that needs UI dependencies removed
        // Implementation should be moved from WPF project after refactoring
    }
}
