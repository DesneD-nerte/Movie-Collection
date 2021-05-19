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
    class AllStudiosViewModel : WorkspaceViewModel
    {
        public ObservableCollection<StudioViewModel> Studios { get; private set; } //Все Студии которые есть
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Фильмы, выбранного актера
        StudioViewModel selectedStudio;
        bool studioSelected;

        DataBaseWork dataBaseStudios;

        public AllStudiosViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllStudiosViewModel_DisplayName;

            dataBaseStudios = dataBase;
            GetAllStudios();
        }

        private void GetAllStudios()
        {
            Studios = new ObservableCollection<StudioViewModel>();
            foreach (var studio in dataBaseStudios.GetStudios())
            {
                var newStudio = new StudioViewModel(studio);

                newStudio.PropertyChanged += OnStudioViewModelPropertyChanged;

                Studios.Add(newStudio);
            }
            Studios.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (StudioViewModel vm in e.NewItems)
                {
                    vm.PropertyChanged += OnStudioViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (StudioViewModel vm in e.OldItems)
                {
                    vm.PropertyChanged -= OnStudioViewModelPropertyChanged;
                }
            }
        }

        private void OnStudioViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as StudioViewModel).VerifyPropertyName(IsSelected);

            if (e.PropertyName == IsSelected)
            {
                selectedStudio = (StudioViewModel)sender;
                studioSelected = true;
                OutputDetailedInformation();
            }
        }

        private void OutputDetailedInformation()
        {
            ObservableCollection<MovieViewModel> movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in selectedStudio.Movies)
            {
                var oneMovie = new MovieViewModel(movie);
                movies.Add(oneMovie);
            }
            Movies = movies;
            base.OnPropertyChanged("Movies");
        }
    }
}
