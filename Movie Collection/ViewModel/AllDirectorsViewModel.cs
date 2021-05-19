using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AllDirectorsViewModel : WorkspaceViewModel
    {
        public ObservableCollection<DirectorViewModel> Directors { get; private set; } //Все режиссеры которые есть
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Фильмы, выбранного режиссера
        DirectorViewModel selectedDirector;
        bool directorSelected;

        DataBaseWork dataBaseDirectors;

        public AllDirectorsViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllDirectorsViewModel_DisplayName;

            dataBaseDirectors = dataBase;
            GetAllDirectors();
        }

        private void GetAllDirectors()
        {
            Directors = new ObservableCollection<DirectorViewModel>();
            foreach (var director in dataBaseDirectors.GetDirectors())
            {
                var newDirector = new DirectorViewModel(director);

                newDirector.PropertyChanged += OnDirectorViewModelPropertyChanged;

                Directors.Add(newDirector);
            }
            Directors.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (DirectorViewModel vm in e.NewItems)
                {
                    vm.PropertyChanged += OnDirectorViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (DirectorViewModel vm in e.OldItems)
                {
                    vm.PropertyChanged -= OnDirectorViewModelPropertyChanged;
                }
            }
        }

        private void OnDirectorViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as DirectorViewModel).VerifyPropertyName(IsSelected);

            if (e.PropertyName == IsSelected)
            {
                selectedDirector = (DirectorViewModel)sender;
                directorSelected = true;
                OutputDetailedInformation();
            }
        }

        private void OutputDetailedInformation()
        {
            ObservableCollection<MovieViewModel> movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in selectedDirector.Movies)
            {
                var oneMovie = new MovieViewModel(movie);
                movies.Add(oneMovie);
            }
            Movies = movies;
            base.OnPropertyChanged("Movies");
        }
    }
}
