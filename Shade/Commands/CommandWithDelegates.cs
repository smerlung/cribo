namespace Shade.Commands
{
    using System;
    using System.Windows.Input;

    public class CommandWithDelegates : ICommand
    {
        private Action execute = null;
        private Action<object> executewithparameter = null;
        private Func<bool> canexecute = null;
        private ICommand commandChangeSignature;
        private Func<bool> p;

        public CommandWithDelegates(Action execute, Func<bool> canexecute)
        {
            this.execute = execute;
            this.canexecute = canexecute;
        }

        public CommandWithDelegates(Action<object> execute, Func<bool> canexecute)
        {
            this.executewithparameter = execute;
            this.canexecute = canexecute;
        }

        public CommandWithDelegates(ICommand commandChangeSignature, Func<bool> p)
        {
            this.commandChangeSignature = commandChangeSignature;
            this.p = p;
        }

        public bool CanExecute(object parameter)
        {
            return this.canexecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                this.execute();
            }
            else
            {
                this.executewithparameter(parameter);
            }
        }
    }
}