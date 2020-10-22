using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SFConnectorFullScreenUI
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged = (sender, e) => { };


        public RelayCommand(Action execute): this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute is null");
            _canExecute = canExecute;

        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || (_canExecute?.Invoke() ?? true);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)
                && _execute != null)
            {
                _execute();
            }
        }

    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        private Action execCreateNewTabFromCases;
        private Func<bool> canCreateNewTabFromCases;

        public event EventHandler CanExecuteChanged = (sender, e) => { };


        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute is null");
            _canExecute = canExecute;

        }

        public RelayCommand(Action execCreateNewTabFromCases, Func<bool> canCreateNewTabFromCases)
        {
            this.execCreateNewTabFromCases = execCreateNewTabFromCases;
            this.canCreateNewTabFromCases = canCreateNewTabFromCases;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || (_canExecute?.Invoke((T)parameter) ?? true);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)
                && _execute != null)
            {
                _execute((T)parameter);
            }
        }

    }




}
