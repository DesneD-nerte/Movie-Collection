using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllMoviesViewModel : WorkspaceViewModel
    {
        public ObservableCollection<Type> AllMovies { get; private set; }
        public string Description { get; set; }
        public ObservableCollection<Type> Actors { get; private set; }
        public ObservableCollection<Type> Directors { get; private set; }
        public ObservableCollection<Type> Genres { get; private set; }
        public ObservableCollection<Type> Storages { get; private set; }
    }
}
