namespace Samsung_Jellyfin_Installer.Shared.Services
{
    /// <summary>
    /// Abstraction for marshalling calls to the UI thread
    /// </summary>
    public interface IUIThreadService
    {
        /// <summary>
        /// Execute an action on the UI thread
        /// </summary>
        Task InvokeOnUIThreadAsync(Action action);
        
        /// <summary>
        /// Execute a function on the UI thread and return the result
        /// </summary>
        Task<T> InvokeOnUIThreadAsync<T>(Func<T> function);
        
        /// <summary>
        /// Check if currently on the UI thread
        /// </summary>
        bool IsOnUIThread { get; }
    }
}
