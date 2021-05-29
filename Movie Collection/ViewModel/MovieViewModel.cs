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
        readonly Movie movie;
        readonly MainWindowViewModel mainWindowViewModel;

        public string Name
        {
            get
            {
                return movie.Name;
            }
            set
            {
                movie.Name = value;
                base.OnPropertyChanged("Name");
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
                base.OnPropertyChanged("Description");
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
                base.OnPropertyChanged("Duration");
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
                base.OnPropertyChanged("CountOfSeries");
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
                base.OnPropertyChanged("Release");
            }
        }

        public void UpdateStorage(Storage storage)
        {
            Storage = new StorageViewModel(storage);
            movie.Storage = storage;
        }

        RelayCommand editCommand;
        RelayCommand doubleClickDeleteStudiosCommand;
        RelayCommand doubleClickDeleteActorsCommand;
        RelayCommand doubleClickDeleteDirectorsCommand;
        RelayCommand doubleClickDeleteGenresCommand;
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
        public ICommand DoubleClickDeleteStudiosCommand
        {
            get
            {
                if (doubleClickDeleteStudiosCommand == null)
                {
                    doubleClickDeleteStudiosCommand = new RelayCommand(param =>
                    {
                        if (SelectedStudio != null)
                        {
                            if (movie.Studios.Contains(SelectedStudio.Studio))
                            {
                                movie.Studios.Remove(SelectedStudio.Studio);
                                Studios.Remove(SelectedStudio);
                            }
                        }
                    }
                    );
                }
                return doubleClickDeleteStudiosCommand;
            }
        }
        public ICommand DoubleClickDeleteActorsCommand
        {
            get
            {
                if (doubleClickDeleteActorsCommand == null)
                {
                    doubleClickDeleteActorsCommand = new RelayCommand(param =>
                    {
                        if (SelectedActor != null)
                        {
                            if (movie.Actors.Contains(SelectedActor.Actor))
                            {
                                movie.Actors.Remove(SelectedActor.Actor);
                                Actors.Remove(SelectedActor);
                            }
                        }
                    }
                    );
                }
                return doubleClickDeleteActorsCommand;
            }
        }
        public ICommand DoubleClickDeleteDirectorsCommand
        {
            get
            {
                if (doubleClickDeleteDirectorsCommand == null)
                {
                    doubleClickDeleteDirectorsCommand = new RelayCommand(param =>
                    {
                        if (SelectedDirector != null)
                        {
                            if (movie.Directors.Contains(SelectedDirector.Director))
                            {
                                movie.Directors.Remove(SelectedDirector.Director);
                                Directors.Remove(SelectedDirector);
                            }
                        }
                    }
                    );
                }
                return doubleClickDeleteDirectorsCommand;
            }
        }
        public ICommand DoubleClickDeleteGenresCommand
        {
            get
            {
                if (doubleClickDeleteGenresCommand == null)
                {
                    doubleClickDeleteGenresCommand = new RelayCommand(param =>
                    {
                        if (SelectedGenre != null)
                        {
                            if (movie.Genres.Contains(SelectedGenre.Genre))
                            {
                                movie.Genres.Remove(SelectedGenre.Genre);
                                Genres.Remove(SelectedGenre);
                            }
                        }
                    }
                    );
                }
                return doubleClickDeleteGenresCommand;
            }
        }
        StudioViewModel selectedStudio;
        ActorViewModel selectedActor;
        DirectorViewModel selectedDirector;
        GenreViewModel selectedGenre;

        public StudioViewModel SelectedStudio
        {
            get => selectedStudio;
            set
            {
                selectedStudio = value;
                base.OnPropertyChanged("SelectedStudio");
            }
        }
        public ActorViewModel SelectedActor
        {
            get => selectedActor;
            set
            {
                selectedActor = value;
                base.OnPropertyChanged("SelectedActor");
            }
        }
        public DirectorViewModel SelectedDirector
        {
            get => selectedDirector;
            set
            {
                selectedDirector = value;
                base.OnPropertyChanged("SelectedDirector");
            }
        }
        public GenreViewModel SelectedGenre
        {
            get => selectedGenre;
            set
            {
                selectedGenre = value;
                base.OnPropertyChanged("SelectedGenre");
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
        public async void AddMovie(DataBaseWork dataBase)
        {
            if (movie.ID == 0)
            {
                foreach (var actorViewModel in Actors)
                {
                    movie.Actors.Add(actorViewModel.Actor);
                }
                foreach (var directorViewModel in Directors)
                {
                    movie.Directors.Add(directorViewModel.Director);
                }

                foreach (var studioViewModel in Studios)
                {
                    movie.Studios.Add(studioViewModel.Studio);
                }

                foreach (var genreViewModel in Genres)
                {
                    movie.Genres.Add(genreViewModel.Genre);
                }

                await dataBase.AddMovie(movie);
            }
            else
            {
                await dataBase.UpdateMovie(movie);
            }
        }
        public async void UpdateMovie(DataBaseWork dataBase)
        {
            await dataBase.UpdateMovie(movie);
        }
        public async void DeleteMovie(DataBaseWork dataBase)
        {
            await dataBase.DeleteMovie(movie);
        }
    }
}
