using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalProcessingView.ViewModel.Base
{
    public class RelayCommand : ICommand
    {
        private Action mAction;

        public RelayCommand(Action mAction)
        {
            this.mAction = mAction;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction();
        }

        public event EventHandler CanExecuteChanged;
    }
}
