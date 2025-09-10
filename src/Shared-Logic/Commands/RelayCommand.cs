using System.ComponentModel;

namespace Samsung_Jellyfin_Installer.Shared.Commands
{
    public interface ICommand
    {
        event EventHandler? CanExecuteChanged;
        bool CanExecute(object? parameter);
        void Execute(object? parameter);
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Generic version for parameterized commands
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) =>
            parameter is T typedParameter ? (_canExecute?.Invoke(typedParameter) ?? true) : false;

        public void Execute(object? parameter)
        {
            if (parameter is T typedParameter)
                _execute(typedParameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
