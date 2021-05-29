using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class AddActorViewModel : WorkspaceViewModel
    {
        public ObservableCollection<CountryViewModel> AllCountry { get; private set; }

        ActorViewModel actor;
        CountryViewModel selectedCountry;

        public ActorViewModel Actor
        {
            get => actor;
            set
            {
                actor = value;
                base.OnPropertyChanged("Actor");
            }
        }
        public CountryViewModel SelectedCountry
        {
            get =>selectedCountry;
            set
            {
                selectedCountry = value;
                actor.UpdateCountry(value.Country);
                base.OnPropertyChanged("SelectedCountry");
            }
        }

        DataBaseWork dataBaseAddActor;

        RelayCommand addActorCommand;

        public ICommand AddActorCommand
        {
            get
            {
                if (addActorCommand == null)
                {
                    addActorCommand = new RelayCommand(param => actor.AddActor(dataBaseAddActor));
                }
                return addActorCommand;
            }
        }

        public AddActorViewModel(DataBaseWork dataBase, ActorViewModel actorViewModel = null)
        {
            if (actorViewModel != null)
            {
                actor = actorViewModel;
                SelectedCountry = new CountryViewModel(actor.Actor.Country);
            }
            else
            {
                actor = new ActorViewModel();
            }

            base.DisplayName = Strings.AddActorViewModel_DisplayName;

            dataBaseAddActor = dataBase;
            FullData();
        }

        private void FullData()
        {
            CreateNewCountries();
        }

        private async void CreateNewCountries()
        {
            List<CountryViewModel> countries = (from db in await dataBaseAddActor.GetCountries() select new CountryViewModel(db)).ToList();
            AllCountry = new ObservableCollection<CountryViewModel>(countries);
        }
    }
}
