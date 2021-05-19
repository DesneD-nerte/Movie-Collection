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
    class AllActorsViewModel : WorkspaceViewModel
    {
        public ObservableCollection<ActorViewModel> Actors { get; private set; } //Все актеры которые есть
        public ObservableCollection<MovieViewModel> Movies { get; private set; }//Фильмы, выбранного актера
        ActorViewModel selectedActor;
        bool actorSelected;

        DataBaseWork dataBaseActors;

        public AllActorsViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllActorsViewModel_DisplayName;

            dataBaseActors = dataBase;
            GetAllActors();
        }

        private void GetAllActors()
        {
            Actors = new ObservableCollection<ActorViewModel>();
            foreach (var actor in dataBaseActors.GetActors())
            {
                var newActor = new ActorViewModel(actor);

                newActor.PropertyChanged += OnActorViewModelPropertyChanged;

                Actors.Add(newActor);
            }
            Actors.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (ActorViewModel vm in e.NewItems)
                {
                    vm.PropertyChanged += OnActorViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (ActorViewModel vm in e.OldItems)
                {
                    vm.PropertyChanged -= OnActorViewModelPropertyChanged;
                }
            }
        }

        private void OnActorViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as ActorViewModel).VerifyPropertyName(IsSelected);

            if (e.PropertyName == IsSelected)
            {
                selectedActor = (ActorViewModel)sender;
                actorSelected = true;
                OutputDetailedInformation();
            }
        }

        private void OutputDetailedInformation()
        {
            ObservableCollection<MovieViewModel> movies = new ObservableCollection<MovieViewModel>();
            foreach (var movie in selectedActor.Movies)
            {
                var oneMovie = new MovieViewModel(movie);
                movies.Add(oneMovie);
            }
            Movies = movies;
            base.OnPropertyChanged("Movies");
        }
    }
}
