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
using System.Threading.Tasks;
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
        private async void GetAllMovies(MainWindowViewModel mainWindowViewModel = null)
        {
            Movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in await dataBaseMovies.GetMovies())
            {
                var newMovie = new MovieViewModel(movie, mainWindowViewModel);

                Movies.Add(newMovie);
            }
        }

        RelayCommand deleteCommand;
        RelayCommand findMovieCommand;
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
        public ICommand FindMovieCommand
        {
            get
            {
                if (findMovieCommand == null)
                {
                    findMovieCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            SelectedMovie = Movies.First(x => x.Name.Contains(SearchMovie));
                        }
                        catch { }
                    });
                }
                return findMovieCommand;
            }
        }

        string searchMovie = "Поиск";
        public string SearchMovie
        {
            get => searchMovie;
            set
            {
                searchMovie = value;
            }
        }
        protected override void OnDispose()
        {

        }
    }
}
