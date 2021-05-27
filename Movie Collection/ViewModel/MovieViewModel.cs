using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class MovieViewModel : WorkspaceViewModel
    {
        Movie movie;
        bool isSelected = false;
        MainWindowViewModel mainWindowViewModel;

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
        public TimeSpan Duration
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
        public DateTime Release
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
        //public string StorageName
        //{
        //    get
        //    {
        //        return movie.Storage.Name;
        //    }
        //    set
        //    {
        //        movie.Storage.Name = value;
        //    }
        //}

        public void UpdateStorage(Storage storage)
        {
            Storage = new StorageViewModel(storage);/////////////////////////////
            movie.Storage = storage;
        }

        RelayCommand editCommand;

        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(param => mainWindowViewModel.ShowEditMovie(this));
                }
                return editCommand;
            }
        }


        public ObservableCollection<ActorViewModel> Actors { get; private set; }
        public ObservableCollection<DirectorViewModel> Directors { get; private set; }
        public ObservableCollection<GenreViewModel> Genres { get; private set; }
        public ObservableCollection<StudioViewModel> Studios { get; private set; }
        public StorageViewModel Storage { get; set; }
        
        public MovieViewModel(Movie newMovie, MainWindowViewModel mainWindowViewModel = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            movie = newMovie;
            Actors = new ObservableCollection<ActorViewModel>(from actor in newMovie.Actors select new ActorViewModel(actor));
            Directors = new ObservableCollection<DirectorViewModel>(from director in newMovie.Directors select new DirectorViewModel(director));
            Genres = new ObservableCollection<GenreViewModel>(from genre in newMovie.Genres select new GenreViewModel(genre));
            Studios = new ObservableCollection<StudioViewModel>(from studio in newMovie.Studios select new StudioViewModel(studio));
            Storage = new StorageViewModel(newMovie.Storage);
        }
        public MovieViewModel()
        {
            movie = new Movie();
            Actors = new ObservableCollection<ActorViewModel>();
            Directors = new ObservableCollection<DirectorViewModel>();
            Genres = new ObservableCollection<GenreViewModel>();
            Studios = new ObservableCollection<StudioViewModel>();
            Storage = new StorageViewModel();
        }

        //в самом классе Movie надо обновть списки
        public void AddMovie(DataBaseWork dataBase)
        {
            foreach (var actorViewModel in Actors)
            {
                movie.Actors.Add(actorViewModel.Actor);
            }
            foreach (var directorViewModel in Directors)
            {
                movie.Directors.Add(directorViewModel.director);
            }

            foreach (var studioViewModel in Studios)
            {
                movie.Studios.Add(studioViewModel.Studio);
            }

            foreach (var genreViewModel in Genres)
            {
                movie.Genres.Add(genreViewModel.Genre);
            }


            if (movie.ID == 0)
            {
                dataBase.AddMovie(movie);
            }
            else
            {
                dataBase.UpdateMovie(movie);
            }
        }
        public void UpdateMovie(DataBaseWork dataBase)
        {
            dataBase.UpdateMovie(movie);
        }
        public void DeleteMovie(DataBaseWork dataBase)
        {
            dataBase.DeleteMovie(movie);
        }
    }
}
