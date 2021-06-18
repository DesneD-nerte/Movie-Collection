using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
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
    class AddMovieViewModel : WorkspaceViewModel
    {
        MovieViewModel movie;

        StudioViewModel selectedStudio;
        ActorViewModel selectedActor;
        DirectorViewModel selectedDirector;
        GenreViewModel selectedGenre;
        StorageViewModel selectedStorage;

        GeneratorMovies generatorMovies = new GeneratorMovies();

        public MovieViewModel Movie
        {
            get => movie;
            set
            {
                movie = value;
                base.OnPropertyChanged("Movie");
            }
        }
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
        public StorageViewModel SelectedStorage
        {
            get => selectedStorage;
            set
            {
                selectedStorage = value;
                movie.UpdateStorage(value.Storage);

                base.OnPropertyChanged("SelectedStorage");
            }
        }

        public ObservableCollection<ActorViewModel> AllActors { get; private set; }
        public ObservableCollection<DirectorViewModel> AllDirectors { get; private set; }
        public ObservableCollection<StudioViewModel> AllStudios { get; private set; }
        public ObservableCollection<GenreViewModel> AllGenres { get; private set; }
        public ObservableCollection<StorageViewModel> AllStorages { get; private set; }

        readonly DataBaseWork dataBaseAddMovie;

        RelayCommand doubleClickStudiosCommand;
        RelayCommand doubleClickActorsCommand;
        RelayCommand doubleClickDirectorsCommand;
        RelayCommand doubleClickGenresCommand;

        RelayCommand addMovieCommand;
        RelayCommand generateMoviesCommand;

        #region Двойной клик для выбора студий, актеров, режиссеров, жанров, накопителя фильма
        public ICommand DoubleClickStudiosCommand
        {
            get
            {
                if (doubleClickStudiosCommand == null)
                {
                    doubleClickStudiosCommand = new RelayCommand(param =>
                        {
                            if (SelectedStudio != null)
                            {
                                if (!Movie.Studios.Any(x => x.Studio.Name == SelectedStudio.Name))
                                {
                                    Movie.Studios.Add(SelectedStudio);
                                }
                            }
                        }
                    );
                }
                return doubleClickStudiosCommand;
            }
        }
        public ICommand DoubleClickActorsCommand
        {
            get
            {
                if (doubleClickActorsCommand == null)
                {
                    doubleClickActorsCommand = new RelayCommand(param =>
                    {
                        if (SelectedActor != null)
                        {
                            if (!Movie.Actors.Any(x => x.Actor.Name == SelectedActor.Name))
                            {
                                Movie.Actors.Add(SelectedActor);
                            }
                        }
                    }
                    );
                }
                return doubleClickActorsCommand;
            }
        }
        public ICommand DoubleClickDirectorsCommand
        {
            get
            {
                if (doubleClickDirectorsCommand == null)
                {
                    doubleClickDirectorsCommand = new RelayCommand(param =>
                    {
                        if (SelectedDirector != null)
                        {
                            if (!Movie.Directors.Any(x => x.Director.Name == SelectedDirector.Name))
                            {
                                Movie.Directors.Add(SelectedDirector);
                            }
                        }
                    }
                    );
                }
                return doubleClickDirectorsCommand;
            }
        }
        public ICommand DoubleClickGenresCommand
        {
            get
            {
                if (doubleClickGenresCommand == null)
                {
                    doubleClickGenresCommand = new RelayCommand(param =>
                    {
                        if (SelectedGenre != null)
                        {
                            if(!Movie.Genres.Any(x => x.Genre.Name == SelectedGenre.Name))
                            {
                                Movie.Genres.Add(SelectedGenre);
                            }
                        }
                    }
                    );
                }
                return doubleClickGenresCommand;
            }
        }
        #endregion

        public ICommand AddMovieCommand
        {
            get
            {
                if (addMovieCommand == null)
                {
                    addMovieCommand = new RelayCommand(param =>
                    {
                          Movie.AddMovie(dataBaseAddMovie);
                    });
                }
                return addMovieCommand;
            }
        }
        public ICommand GenerateMoviesCommand
        {
            get
            {
                if (generateMoviesCommand == null)
                {
                    generateMoviesCommand = new RelayCommand(param =>
                    {
                        generatorMovies.CreateNewMovies(dataBaseAddMovie, 10);
                    });
                }
                return generateMoviesCommand;
            }
        }

        public AddMovieViewModel(DataBaseWork dataBase, MovieViewModel movieViewModel = null)
        {
            movie = movieViewModel ?? new MovieViewModel();

            base.DisplayName = Strings.AddMovieViewModel_DisplayName;

            dataBaseAddMovie = dataBase;
            FullMainEntities();

            if (movieViewModel != null)
            {
                AssignSelectedEntities(Movie);
            }
        }

        private void AssignSelectedEntities(MovieViewModel movie)
        {
             selectedStorage = AllStorages.First(x => x.ID == movie.Storage.ID);
        }

        private void FullMainEntities()
        {
            var task1 = CreateNewActors();
            var task2 = CreateNewDirectors();
            var task3 = CreateNewStudios();
            var task4 = CreateNewGenres();
            var task5 = CreateNewStorages();

            Task.WaitAll(task1, task2, task3, task4, task5);
        }

        private Task CreateNewActors()
        {
            return Task.Run(async() =>
            {
                List<ActorViewModel> actors = (from db in await dataBaseAddMovie.GetActors() select new ActorViewModel(db)).ToList();
                AllActors = new ObservableCollection<ActorViewModel>(actors);
            });
        }
        private Task CreateNewDirectors()
        {
            return Task.Run(async () =>
            {
                List<DirectorViewModel> directors = (from db in await dataBaseAddMovie.GetDirectors() select new DirectorViewModel(db)).ToList();
                AllDirectors = new ObservableCollection<DirectorViewModel>(directors);
            });
        }
        private Task CreateNewStudios()
        {
            return Task.Run(async () =>
            {
                List<StudioViewModel> studios = (from db in await dataBaseAddMovie.GetStudios() select new StudioViewModel(db)).ToList();
                AllStudios = new ObservableCollection<StudioViewModel>(studios);
            });
        }
        private Task CreateNewGenres()
        {
            return Task.Run(async () =>
            {
                List<GenreViewModel> genres = (from db in await dataBaseAddMovie.GetGenres() select new GenreViewModel(db)).ToList();
                AllGenres = new ObservableCollection<GenreViewModel>(genres);
            });
        }
        private Task CreateNewStorages()
        {
            return Task.Run(async() =>
            {
                List<StorageViewModel> storages = (from db in await dataBaseAddMovie.GetStorages() select new StorageViewModel(db)).ToList();
                AllStorages = new ObservableCollection<StorageViewModel>(storages);
            });
        }
    }
}
