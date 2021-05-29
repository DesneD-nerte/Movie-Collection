using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class AddDirectorViewModel : WorkspaceViewModel
    {
        public ObservableCollection<CountryViewModel> AllCountry { get; private set; }

        DirectorViewModel director;
        CountryViewModel selectedCountry;

        public DirectorViewModel Director
        {
            get => director;
            set
            {
                director = value;
                base.OnPropertyChanged("Actor");
            }
        }
        public CountryViewModel SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                director.UpdateCountry(value.Country);
                base.OnPropertyChanged("SelectedCountry");
            }
        }

        DataBaseWork dataBaseAddDirector;

        RelayCommand addDirectorCommand;

        public ICommand AddDirectorCommand
        {
            get
            {
                if (addDirectorCommand == null)
                {
                    addDirectorCommand = new RelayCommand(param => director.AddDirector(dataBaseAddDirector));
                }
                return addDirectorCommand;
            }
        }

        public AddDirectorViewModel(DataBaseWork dataBase, DirectorViewModel directorViewModel = null)
        {
            if (directorViewModel != null)
            {
                director = directorViewModel;
                SelectedCountry = new CountryViewModel(director.Director.Country);
            }
            else
            {
                director = new DirectorViewModel();
            }

            base.DisplayName = Strings.AddDirectorViewModel_DisplayName;

            dataBaseAddDirector = dataBase;
            FullData();
        }

        private void FullData()
        {
            CreateNewCountries();
        }

        private async void CreateNewCountries()
        {
            List<CountryViewModel> countries = (from db in await dataBaseAddDirector.GetCountries() select new CountryViewModel(db)).ToList();
            AllCountry = new ObservableCollection<CountryViewModel>(countries);
        }
    }
}
