using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    public class MovieViewModel : WorkspaceViewModel, IDataErrorInfo
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
        public string Duration
        {
            get
            {
                return movie.Duration.ToString();
            }
            set
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(value, out timeSpan))
                {
                    movie.Duration = timeSpan;
                }
                else
                {
                    movie.Duration = null;
                }

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
        public string Release
        {
            get
            {
                return movie.Release.ToString();
            }
            set
            {
                DateTime dateTime;
                if (DateTime.TryParse(value, out dateTime))
                {
                    movie.Release = dateTime;
                }
                else
                {
                    movie.Release = null;
                }
                base.OnPropertyChanged("Release");
            }
        }

        public void UpdateStorage(Storage storage)
        {
            Storage = new StorageViewModel(storage);
            movie.Storage = storage;
        }

        #region Команды редактирования фильма и удаления сущностей двойным кликом
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
        #endregion

        RelayCommand setNullCommand;
        public ICommand SetNullCommand
        {
            get
            {
                if (setNullCommand == null)
                {
                    setNullCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            
                        }
                        catch { }
                    });
                }
                return setNullCommand;
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


        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public string Error
        {
            get => ((IDataErrorInfo)movie).Error;
        }

        public string this[string propertyName]
        {
            get
            {
               string error = ((IDataErrorInfo)movie)[propertyName];

                if (ErrorCollection.ContainsKey(propertyName))
                    ErrorCollection[propertyName] = error;
                else if (error != null)
                    ErrorCollection.Add(propertyName, error);

                OnPropertyChanged("ErrorCollection");

                return error;
            }
        }

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
            try
            {
                if (movie.CheckPropertiesBeforeAdding())
                {
                    await StartAdding(dataBase);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private Task StartAdding(DataBaseWork dataBase)
        {
            return Task.Run(async () =>
            {
                foreach (var actorViewModel in Actors)
                {
                    if (movie.Actors.All(x => x.ID != actorViewModel.Actor.ID))
                    {
                        movie.Actors.Add(actorViewModel.Actor);
                    }
                }
                foreach (var directorViewModel in Directors)
                {
                    if (movie.Directors.All(x => x.ID != directorViewModel.Director.ID))
                    {
                        movie.Directors.Add(directorViewModel.Director);
                    }
                }
                foreach (var studioViewModel in Studios)
                {
                    if (movie.Studios.All(x => x.ID != studioViewModel.Studio.ID))
                    {
                        movie.Studios.Add(studioViewModel.Studio);
                    }
                }
                foreach (var genreViewModel in Genres)
                {
                    if (movie.Genres.All(x => x.ID != genreViewModel.Genre.ID))
                    {
                        movie.Genres.Add(genreViewModel.Genre);
                    }
                }

                if (movie.ID == 0)
                {
                    await dataBase.AddMovie(movie);
                }
                else
                {
                    await dataBase.UpdateMovie(movie);
                }
            });
        }
        public async void DeleteMovie(DataBaseWork dataBase)
        {
            await dataBase.DeleteMovie(movie);
        }
    }
}
