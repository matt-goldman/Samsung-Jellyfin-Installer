// TODO: Technical Debt - SamsungLoginService has WPF Dependencies
// This service directly uses:
// - Application.Current.Dispatcher
// - SamsungLoginWindow (WPF View)  
// - System.Windows imports
//
// Actions needed:
// 1. Create abstraction for UI thread marshalling
// 2. Move login window logic to appropriate UI layer
// 3. Use dependency injection for dialog/window services
// 4. Remove direct WPF dependencies

using Samsung_Jellyfin_Installer.Shared.Models;

namespace Samsung_Jellyfin_Installer.Shared.Services
{
    // Placeholder - SamsungLoginService needs UI dependencies removed
    public interface ISamsungLoginService
    {
        Task<SamsungAuth> PerformSamsungLoginAsync();
    }
}
