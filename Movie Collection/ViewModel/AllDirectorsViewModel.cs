using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllDirectorsViewModel : WorkspaceViewModel
    {
        public ObservableCollection<Type> AllDirectors { get; private set; }
        public ObservableCollection<Type> AllMovies { get; private set; }
        public AllDirectorsViewModel()
        {
            base.DisplayName = Strings.AllDirectorsViewModel_DisplayName;
        }
    }
}
