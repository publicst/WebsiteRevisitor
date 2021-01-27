using System;
using System.Windows.Input;

namespace WebsiteRevisitor
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other object by invoking delegates.
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Declaration
        readonly Func<bool> _canExecute;
        readonly Action _execute;
        #endregion

        #region Constructor
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region IComand Members
        /// <summary>
        /// Note : What is CanExecuteChanged event?
        ///   - This event is raised by the command to notify it's consumers (i.e. Button) that it's CanExecute property may have changed. 
        ///     So if focus is moved from one TextBox to another, your command may need to be enabled/disabled. 
        ///     This information also needs to be passed to any controls using your command. 
        ///     In general, this event reexposes CommandManager.RequerySuggested event. https://stackoverflow.com/a/6426414/1076116
        ///   Note : What is CommandManager.RequerySuggested?
        ///     - The RequerySuggested event is fired quite often, as focus is moved, text selection is changed. 
        ///       This can also be manually raised by calling CommandManager.InvalidateRequerySuggested.
        ///     Really deep Note : 
        ///       - RequerySuggested states this event is static, so it will only hold on to the handler with weak reference.
        ///         So object that listens to this event should hold strong reference to their event handler.
        ///       - However, John Smith's famous implementation does not. Why? Because -> https://stackoverflow.com/a/8313483/1076116 
        ///          - In a nutshell WPF visuals are not capable of unhooking the visual elements very well.
        ///            Meaning the event handler will remain dangling and cause memory leak if visuals dynamic.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { if (_canExecute != null) CommandManager.RequerySuggested += value; }
            remove { if (_canExecute != null) CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
        #endregion 
    }
}
