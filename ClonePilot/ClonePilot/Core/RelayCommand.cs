using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClonePilot.Core
{
    /// <summary>
    /// A basic command that runs an action
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public class RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : ICommand
    {
        /// <summary>
        /// The action to run
        /// </summary>
        private readonly Action<object> _execute = execute;
        /// <summary>
        /// The function to run to determine if the command can execute
        /// </summary>
        private readonly Func<object, bool> _canExecute = canExecute;

        /// <summary>
        /// Event that is fired when the <see cref="CanExecute(object)"/> value changes
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Determines whether the command can execute in its current state
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute is null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
