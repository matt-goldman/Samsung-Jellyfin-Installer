using Samsung_Jellyfin_Installer.Shared.Services;

namespace DesktopUI.Services
{
    public class MauiUIThreadService : IUIThreadService
    {
        public bool IsOnUIThread => MainThread.IsMainThread;

        public async Task InvokeOnUIThreadAsync(Action action)
        {
            if (IsOnUIThread)
            {
                action();
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(action);
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
                return await MainThread.InvokeOnMainThreadAsync(func);
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
                await MainThread.InvokeOnMainThreadAsync(asyncAction);
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
                return await MainThread.InvokeOnMainThreadAsync(asyncFunc);
            }
        }
    }
}
