using Movie_Collection.Model;
using Movie_Collection.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.View
{
    class MovieViewModel : WorkspaceViewModel
    {
        Movie movie;

        public string Name
        {
            get
            {
                return movie.Name;
            }
            set
            {
                movie.Name = value;
            }
        }
        public string Duration
        {
            get
            {
                return movie.Duration;
            }
            set
            {
                movie.Duration = value;
            }
        }
        public int CountOfSeries
        {
            get
            {
                return movie.CountOfSeries;
            }
            set
            {
                movie.CountOfSeries = value;
            }
        }
        public string Release
        {
            get
            {
                return movie.Release;
            }
            set
            {
                movie.Release = value;
            }
        }
        public string StorageName
        {
            get
            {
                return movie.Storage.Name;
            }
            set
            {
                movie.Storage.Name = value;
            }
        }


        public ObservableCollection<Movie> Movies { get; private set; }
        public string Description { get; set; }
        public ObservableCollection<Actor> Actors { get; private set; }
        public ObservableCollection<Director> Directors { get; private set; }
        public ObservableCollection<Genre> Genres { get; private set; }
        public ObservableCollection<Studio> Studios { get; private set; }

        public MovieViewModel(Movie newMovie)
        {
            movie = newMovie;
            //movie = new Movie(1, "ABOBA", "Hello, it's  you daily dose of the internet", null, 1, "03:00:15", "Завтра");
        }
    }
}
