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
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Перечисляются все фильмы для главной таблицы

        MovieViewModel selectedMovie;

        public MovieViewModel SelectedMovie
        {
            get => selectedMovie;
            set
            {
                selectedMovie = value;
                base.OnPropertyChanged("SelectedMovie");
            }
        }

        DataBaseWork dataBaseMovies;

        public AllMoviesViewModel(DataBaseWork dataBase, MainWindowViewModel mainWindowViewModel = null)
        {
            selectedMovie = new MovieViewModel();

            base.DisplayName = Strings.AllMoviesViewModel_DisplayName;

            dataBaseMovies = dataBase;
            GetAllMovies(mainWindowViewModel);
        }

        //Добавление всех фильмов на таблицу поочередно
        private void GetAllMovies(MainWindowViewModel mainWindowViewModel = null)
        {
            Movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in dataBaseMovies.GetMovies())
            {
                var newMovie = new MovieViewModel(movie, mainWindowViewModel);

              //  newMovie.PropertyChanged += OnMovieViewModelPropertyChanged;

                Movies.Add(newMovie);
            }
           // Movies.CollectionChanged += OnCollectionChanged;
        }

        RelayCommand deleteCommand;
        
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(param =>
                    {
                        if (selectedMovie != null)
                        {
                            selectedMovie.DeleteMovie(dataBaseMovies);
                            Movies.Remove(selectedMovie);
                        }
                    });
                }
                return deleteCommand;
            }
        }








        //bool movieSelected;
        //public string Description 
        //{ get => description; 
        //    set => description = value; }                                   //Начиная с этого свойства идет
        //public ObservableCollection<ActorViewModel> Actors { get; private set; }//описание конкретного фильма 
        //public ObservableCollection<DirectorViewModel> Directors { get; private set; }
        //public ObservableCollection<GenreViewModel> Genres { get; private set; }
        //public ObservableCollection<StudioViewModel> Studios { get; private set; }





        //private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null && e.NewItems.Count != 0)
        //    {
        //        foreach (MovieViewModel vm in e.NewItems)
        //        {
        //            vm.PropertyChanged += OnMovieViewModelPropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null && e.OldItems.Count != 0)
        //    {
        //        foreach (MovieViewModel vm in e.OldItems)
        //        {
        //            vm.PropertyChanged -= OnMovieViewModelPropertyChanged;
        //        }
        //    }
        //}

        ////Если происходит изменение какого-либо свойства у экземпляра класса "MovieViewModel"
        //void OnMovieViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    string IsSelected = "IsSelected";

        //    (sender as MovieViewModel).VerifyPropertyName(IsSelected);

        //    if (e.PropertyName == IsSelected)
        //    {
        //        SelectedMovie = (MovieViewModel)sender;
        //        ////movieSelected = true;
        //        OutputDetailedInformation();
        //    }
        //}

        //private void OutputDetailedInformation()
        //{
        //    Description = SelectedMovie.Description;
        //    base.OnPropertyChanged("Description");

        //    ObservableCollection<ActorViewModel> actors = new ObservableCollection<ActorViewModel>();
        //    foreach (var actor in SelectedMovie.Actors)
        //    {
        //        var oneActor = new ActorViewModel(actor);
        //        actors.Add(oneActor);
        //    }
        //    Actors = actors;
        //    base.OnPropertyChanged("Actors");


        //    ObservableCollection<DirectorViewModel> directors = new ObservableCollection<DirectorViewModel>();
        //    foreach (var director in SelectedMovie.Directors)
        //    {
        //        var oneDirector = new DirectorViewModel(director);
        //        directors.Add(oneDirector);
        //    }
        //    Directors = directors;
        //    base.OnPropertyChanged("Directors");


        //    ObservableCollection<StudioViewModel> studios = new ObservableCollection<StudioViewModel>();
        //    foreach (var studio in SelectedMovie.Studios)
        //    {
        //        var oneStudio = new StudioViewModel(studio);
        //        studios.Add(oneStudio);
        //    }
        //    Studios = studios;
        //    base.OnPropertyChanged("Studios");


        //    ObservableCollection<GenreViewModel> genres = new ObservableCollection<GenreViewModel>();
        //    foreach (var genre in SelectedMovie.Genres)
        //    {
        //        var oneGenre = new GenreViewModel(genre);
        //        genres.Add(oneGenre);
        //    }
        //    Genres = genres;
        //    base.OnPropertyChanged("Genres");
        //}

        protected override void OnDispose()
        {

        }
    }
}
