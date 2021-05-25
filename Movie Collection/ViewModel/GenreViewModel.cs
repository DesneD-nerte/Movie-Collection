using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class GenreViewModel : WorkspaceViewModel
    {
        internal Genre Genre { get; set; }

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public GenreViewModel(Genre newGenre)
        {
            Genre = newGenre;
            Movies = new ObservableCollection<MovieViewModel>((from movie in newGenre.Movies select new MovieViewModel(movie)));
        }

        public string Name
        {
            get
            {
                return Genre.Name;
            }
            set
            {
                Genre.Name = value;
            }
        }

        public void DeleteGenre(DataBaseWork dataBase)
        {
            dataBase.DeleteGenre(Genre);
        }
    }
}
