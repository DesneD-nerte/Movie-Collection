using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllActorsViewModel : WorkspaceViewModel
    {
        public ObservableCollection<Type> AllActors { get; private set; }
        public ObservableCollection<Type> AllMovies { get; private set; }
        public AllActorsViewModel()
        {
            base.DisplayName = Strings.AllActorsViewModel_DisplayName;
        }
    }
}
