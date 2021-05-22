using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                Movie.Storage = value;
                base.OnPropertyChanged("SelectedStorage");
            }
        }

        public ObservableCollection<ActorViewModel> AllActors { get; private set; }
        public ObservableCollection<DirectorViewModel> AllDirectors { get; private set; }
        public ObservableCollection<StudioViewModel> AllStudios { get; private set; }
        public ObservableCollection<GenreViewModel> AllGenres { get; private set; }
        public ObservableCollection<StorageViewModel> AllStorages { get; private set; }


        DataBaseWork dataBaseAddMovie;

        RelayCommand doubleClickStudiosCommand;
        RelayCommand doubleClickActorsCommand;
        RelayCommand doubleClickDirectorsCommand;
        RelayCommand doubleClickGenresCommand;

        RelayCommand addMovieCommand;

        #region Двойной клик для выбора студий, актеров, режиссеров, жанров фильма
        public ICommand DoubleClickStudiosCommand
        {
            get
            {
                if(doubleClickStudiosCommand == null)
                {
                    doubleClickStudiosCommand = new RelayCommand(param =>
                        {
                            if (!movie.Studios.Contains(SelectedStudio))
                            {
                                Movie.Studios.Add(SelectedStudio);
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
                        if (!Movie.Actors.Contains(SelectedActor))
                        {
                            Movie.Actors.Add(SelectedActor);
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
                        if (!Movie.Directors.Contains(SelectedDirector))
                        {
                            Movie.Directors.Add(SelectedDirector);
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
                        if (!Movie.Genres.Contains(SelectedGenre))
                        {
                            Movie.Genres.Add(SelectedGenre);
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
                    addMovieCommand = new RelayCommand(param => Movie.AddMovie(dataBaseAddMovie));
                }
                return addMovieCommand;
            }
        }

        /// <summary>
        /// Начинается процесс создания нового фильма с помощью обращения в базу данных
        /// </summary>
        /// <param name="dataBase"></param>
        public AddMovieViewModel(DataBaseWork dataBase)
        {
            movie = new MovieViewModel();

            base.DisplayName = Strings.AddMovieViewModel_DisplayName;

            dataBaseAddMovie = dataBase;
            FullData();
        }

        private void FullData()
        {
            CreateNewActors();
            CreateNewDirectors();
            CreateNewStudios();
            CreateNewGenres();
            CreateNewStorages();
        }

        private void CreateNewActors()
        {
            List<ActorViewModel> actors = (from db in dataBaseAddMovie.GetActors() select new ActorViewModel(db)).ToList();
            AllActors = new ObservableCollection<ActorViewModel>(actors);
        }
        private void CreateNewDirectors()
        {
            List<DirectorViewModel> directors = (from db in dataBaseAddMovie.GetDirectors() select new DirectorViewModel(db)).ToList();
            AllDirectors = new ObservableCollection<DirectorViewModel>(directors);
        }
        private void CreateNewStudios()
        {
            List<StudioViewModel> studios = (from db in dataBaseAddMovie.GetStudios() select new StudioViewModel(db)).ToList();
            AllStudios = new ObservableCollection<StudioViewModel>(studios);
        }
        private void CreateNewGenres()
        {
            List<GenreViewModel> genres = (from db in dataBaseAddMovie.GetGenres() select new GenreViewModel(db)).ToList();
            AllGenres = new ObservableCollection<GenreViewModel>(genres);
        }
        private void CreateNewStorages()
        {
            List<StorageViewModel> storages = (from db in dataBaseAddMovie.GetStorages() select new StorageViewModel(db)).ToList();
            AllStorages = new ObservableCollection<StorageViewModel>(storages);
        }
    }
}
