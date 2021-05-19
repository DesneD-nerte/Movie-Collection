using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllGenresViewModel : WorkspaceViewModel
    {
        public ObservableCollection<GenreViewModel> Genres { get; private set; } //Все актеры которые есть
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Фильмы, выбранного актера
        GenreViewModel selectedGenre;
        bool genreSelected;

        DataBaseWork dataBaseGenres;

        public AllGenresViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllGenresViewModel_DisplayName;

            dataBaseGenres = dataBase;
            GetAllGenres();
        }

        private void GetAllGenres()
        {
            Genres = new ObservableCollection<GenreViewModel>();
            foreach (var genre in dataBaseGenres.GetGenres())
            {
                var newGenre = new GenreViewModel(genre);

                newGenre.PropertyChanged += OnGenreViewModelPropertyChanged;

                Genres.Add(newGenre);
            }
            Genres.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (GenreViewModel vm in e.NewItems)
                {
                    vm.PropertyChanged += OnGenreViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (GenreViewModel vm in e.OldItems)
                {
                    vm.PropertyChanged -= OnGenreViewModelPropertyChanged;
                }
            }
        }

        private void OnGenreViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as GenreViewModel).VerifyPropertyName(IsSelected);

            if (e.PropertyName == IsSelected)
            {
                selectedGenre = (GenreViewModel)sender;
                genreSelected = true;
                OutputDetailedInformation();
            }
        }

        private void OutputDetailedInformation()
        {
            ObservableCollection<MovieViewModel> movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in selectedGenre.Movies)
            {
                var oneMovie = new MovieViewModel(movie);
                movies.Add(oneMovie);
            }
            Movies = movies;
            base.OnPropertyChanged("Movies");
        }
    }
}
