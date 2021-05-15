using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class WorkspaceViewModel : ViewModelBase
    {
        RelayCommand closeCommand;

        protected WorkspaceViewModel()
        {

        }

        public ICommand CloseCommand
        {
            get
            {
                if(closeCommand != null)
                {
                    closeCommand = new RelayCommand(param => this.OnRequestClose());
                }
                return closeCommand;
            }
        }

        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if(handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
