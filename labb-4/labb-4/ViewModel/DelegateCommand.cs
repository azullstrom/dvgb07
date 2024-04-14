using System;
using System.Windows.Input;

namespace labb_4.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        private readonly Action<object> _executeWithParam;
        private readonly Func<object, bool> _canExecuteWithParam;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParam = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            else if (_canExecuteWithParam != null)
            {
                return _canExecuteWithParam(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
            else if (_executeWithParam != null)
            {
                _executeWithParam(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
