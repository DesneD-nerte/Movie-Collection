using Movie_Collection.Model;
using Movie_Collection.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class MovieViewModel : WorkspaceViewModel
    {
        readonly Movie movie;
        bool isSelected = false;

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
        public string Description
        {
            get
            {
                return movie.Description;
            }
            set
            {
                movie.Description = value;
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
                //return "Hdd";
                return movie.Storage.Name;
            }
            set
            {
                movie.Storage.Name = value;
            }
        }

        public bool IsSelected
        {
            get 
            { 
                return isSelected; 
            }
            set
            {
                if (value == IsSelected)
                {
                    return;
                }
                isSelected = value;
                base.OnPropertyChanged("IsSelected");
            }
        }

        public List<Actor> Actors { get; private set; }
        public List<Director> Directors { get; private set; }
        public List<Genre> Genres { get; private set; }
        public List<Studio> Studios { get; private set; }

        public MovieViewModel(Movie newMovie)
        {
            movie = newMovie;
            Actors = newMovie.Actors;
            Directors = newMovie.Directors;
            Genres = newMovie.Genres;
            Studios = newMovie.Studios;

            //movie = new Movie(1, "ABOBA", "Hello, it's  you daily dose of the internet", null, 1, "03:00:15", "Завтра");
        }
    }
}
