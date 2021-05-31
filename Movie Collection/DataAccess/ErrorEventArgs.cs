using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.DataAccess
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public ErrorEventArgs(string errorMessage)
        {
            Message = errorMessage;
        }
    }
}
