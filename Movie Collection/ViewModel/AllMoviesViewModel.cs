using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using Movie_Collection.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class AllMoviesViewModel : WorkspaceViewModel
    {
        //bool IsSelected = false;
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Перечисляются все фильмы для главной таблицы

        MovieViewModel selectedMovie;
        bool movieSelected;
        public string Description { get; set; }                                   //Начиная с этого свойства идет
        public ObservableCollection<ActorViewModel> Actors { get; private set; }//описание конкретного фильма 
        public ObservableCollection<DirectorViewModel> Directors { get; private set; }
        public ObservableCollection<GenreViewModel> Genres { get; private set; }
        public ObservableCollection<StudioViewModel> Studios { get; private set; }

        DataBaseWork dataBaseMovies;

        RelayCommand editCommand;
        RelayCommand deleteCommand;
        #region Команды
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(param => MessageBox.Show("Nothing", "Info",
                                                                                MessageBoxButton.OK,
                                                                                MessageBoxImage.Information));
                }
                return editCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(param => MessageBox.Show("Nothing", "Info", MessageBoxButton.OK, MessageBoxImage.Information));
                }
                return deleteCommand;
            }
        }
        #endregion

        public AllMoviesViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllMoviesViewModel_DisplayName;

            dataBaseMovies = dataBase;
            GetAllMovies();
        }

        //Добавление всех фильмов на таблицу поочередно
        private void GetAllMovies()
        {
            Movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in dataBaseMovies.GetMovies())
            {
                var newMovie = new MovieViewModel(movie);

                newMovie.PropertyChanged += OnMovieViewModelPropertyChanged;

                Movies.Add(newMovie);
            }
            Movies.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (MovieViewModel vm in e.NewItems)
                {
                    vm.PropertyChanged += OnMovieViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (MovieViewModel vm in e.OldItems)
                {
                    vm.PropertyChanged -= OnMovieViewModelPropertyChanged;
                }
            }
        }

        //Если происходит изменение какого-либо свойства у экземпляра класса "MovieViewModel"
        void OnMovieViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as MovieViewModel).VerifyPropertyName(IsSelected);

            if (e.PropertyName == IsSelected)
            {
                selectedMovie = (MovieViewModel)sender;
                //movieSelected = true;
                OutputDetailedInformation();
            }
        }

        private void OutputDetailedInformation()
        {
            Description = selectedMovie.Description;
            base.OnPropertyChanged("Description");

            ObservableCollection<ActorViewModel> actors = new ObservableCollection<ActorViewModel>();
            foreach (var actor in selectedMovie.Actors)
            {
                var oneActor = new ActorViewModel(actor);
                actors.Add(oneActor);
            }
            Actors = actors;
            base.OnPropertyChanged("Actors");


            ObservableCollection<DirectorViewModel> directors = new ObservableCollection<DirectorViewModel>();
            foreach (var director in selectedMovie.Directors)
            {
                var oneDirector = new DirectorViewModel(director);
                directors.Add(oneDirector);
            }
            Directors = directors;
            base.OnPropertyChanged("Directors");


            ObservableCollection<StudioViewModel> studios = new ObservableCollection<StudioViewModel>();
            foreach (var studio in selectedMovie.Studios)
            {
                var oneStudio = new StudioViewModel(studio);
                studios.Add(oneStudio);
            }
            Studios = studios;
            base.OnPropertyChanged("Studios");


            ObservableCollection<GenreViewModel> genres = new ObservableCollection<GenreViewModel>();
            foreach (var genre in selectedMovie.Genres)
            {
                var oneGenre = new GenreViewModel(genre);
                genres.Add(oneGenre);
            }
            Genres = genres;
            base.OnPropertyChanged("Genres");
        }

        protected override void OnDispose()
        {
            
        }
    }
}
