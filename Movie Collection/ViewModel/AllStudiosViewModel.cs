using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllStudiosViewModel
    {
        public ObservableCollection<Type> AllStudios { get; private set; }
        public ObservableCollection<Type> AllMovies { get; private set; }
    }
}
