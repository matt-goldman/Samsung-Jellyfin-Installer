using Samsung_Jellyfin_Installer.Shared.Services;
using System.Windows;
using System.Windows.Threading;

namespace Samsung_Jellyfin_Installer.WPF.Services
{
    /// <summary>
    /// WPF-specific implementation of UI thread marshalling service
    /// </summary>
    public class WpfUIThreadService : IUIThreadService
    {
        private readonly Dispatcher _dispatcher;

        public WpfUIThreadService()
        {
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public bool IsOnUIThread => _dispatcher.CheckAccess();

        public async Task InvokeOnUIThreadAsync(Action action)
        {
            if (IsOnUIThread)
            {
                action();
            }
            else
            {
                await _dispatcher.InvokeAsync(action);
            }
        }

        public async Task<T> InvokeOnUIThreadAsync<T>(Func<T> func)
        {
            if (IsOnUIThread)
            {
                return func();
            }
            else
            {
                return await _dispatcher.InvokeAsync(func);
            }
        }

        public async Task InvokeOnUIThreadAsync(Func<Task> asyncAction)
        {
            if (IsOnUIThread)
            {
                await asyncAction();
            }
            else
            {
                await _dispatcher.InvokeAsync(async () => await asyncAction());
            }
        }

        public async Task<T> InvokeOnUIThreadAsync<T>(Func<Task<T>> asyncFunc)
        {
            if (IsOnUIThread)
            {
                return await asyncFunc();
            }
            else
            {
                var result = await _dispatcher.InvokeAsync(asyncFunc);
                return await result;
            }
        }
    }
}
