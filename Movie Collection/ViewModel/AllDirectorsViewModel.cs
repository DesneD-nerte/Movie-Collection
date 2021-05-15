using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllDirectorsViewModel
    {
        public ObservableCollection<Type> AllDirectors { get; private set; }
        public ObservableCollection<Type> AllMovies { get; private set; }
    }
}
