using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using Movie_Collection.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllMoviesViewModel : WorkspaceViewModel
    {
        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public string Description { get; set; }
        public ObservableCollection<Actor> Actors { get; private set; }
        public ObservableCollection<Director> Directors { get; private set; }
        public ObservableCollection<Genre> Genres { get; private set; }
        public ObservableCollection<Studio> Studios { get; private set; }

        DataBaseWork dataBaseMovies;
        
        public AllMoviesViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllMoviesViewModel_DisplayName;

            dataBaseMovies = dataBase;
            GetAllMovies();
        }

        private void GetAllMovies()
        {
            foreach (var movie in dataBaseMovies.GetMovies())
            {
                Movies.Add(new MovieViewModel(movie));
            }
        }
    }
}
