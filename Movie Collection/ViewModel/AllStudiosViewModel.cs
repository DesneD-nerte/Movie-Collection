using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllStudiosViewModel : WorkspaceViewModel
    {
        public ObservableCollection<Type> AllStudios { get; private set; }
        public ObservableCollection<Type> AllMovies { get; private set; }
        public AllStudiosViewModel()
        {
            base.DisplayName = Strings.AllStudiosViewModel_DisplayName;
        }
    }
}
