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
    class AddStudioViewModel : WorkspaceViewModel
    {
        StudioViewModel studio;
        CountryViewModel selectedCountry;

        public StudioViewModel Studio
        {
            get => studio;
            set
            {
                studio = value;
                base.OnPropertyChanged("Studio");
            }
        }
        public CountryViewModel SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                Studio.UpdateCountry(value.Country);

                base.OnPropertyChanged("SelectedCountry");
            }
        }
        RelayCommand addStudioCommand;

        public ICommand AddStudioCommand
        {
            get
            {
                if (addStudioCommand == null)
                {
                    addStudioCommand = new RelayCommand(param => Studio.AddStudio(dataBaseAddStudio));
                }
                return addStudioCommand;
            }
        }

        public ObservableCollection<CountryViewModel> AllCountry { get; private set; }
        DataBaseWork dataBaseAddStudio;

        public AddStudioViewModel(DataBaseWork dataBase, StudioViewModel studioViewModel = null)
        {
            if (studioViewModel != null)
            {
                Studio = studioViewModel;
                SelectedCountry = new CountryViewModel(Studio.Country.Country);
            }
            else
            {
                Studio = new StudioViewModel();
            }

            base.DisplayName = Strings.AddStudioViewModel_DisplayName;
            
            dataBaseAddStudio = dataBase;

            CreateNewsCountries();
        }
        private void CreateNewsCountries()
        {
            List<CountryViewModel> studios = (from db in dataBaseAddStudio.GetCountries() select new CountryViewModel(db)).ToList();
            AllCountry = new ObservableCollection<CountryViewModel>(studios);
        }
    }
}
