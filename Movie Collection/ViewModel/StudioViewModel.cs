using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    public class StudioViewModel : WorkspaceViewModel
    {
        internal Studio Studio { get; set; }
        MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public CountryViewModel Country { get; set; }
        public string Name
        {
            get
            {
                return Studio.Name;
            }
            set
            {
                Studio.Name = value;
            }
        }
        //public string CountryName/////////////////////////////////////////////
        //{
        //    get
        //    {
        //        return Studio.Country.Name;
        //    }
        //    set
        //    {
        //        Studio.Country.Name = value;
        //    }
        //}

        RelayCommand editStudioCommand;

        public ICommand EditStudioCommand
        {
            get
            {
                if (editStudioCommand == null)
                {
                    editStudioCommand = new RelayCommand(param => mainWindowViewModel.ShowEditStudio(this));
                }
                return editStudioCommand;
            }
        }


        public void UpdateCountry(Country country)
        {
            Country = new CountryViewModel(country);
            Studio.Country = country;
        }

        public StudioViewModel(Studio newStudio, MainWindowViewModel mainWindowViewModel = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            Studio = newStudio;
            Movies = new ObservableCollection<MovieViewModel>((from movie in newStudio.Movies select new MovieViewModel(movie)));
            Country = new CountryViewModel(newStudio.Country);
        }

        public StudioViewModel()
        {
            Studio = new Studio();
            Movies = new ObservableCollection<MovieViewModel>();
            Country = new CountryViewModel();
        }

        public void AddStudio(DataBaseWork dataBase)
        {
            if (Studio.ID == 0)
            {
                dataBase.AddStudio(Studio);
            }
            else
            {
                dataBase.UpdateStudio(Studio);
            }
        }
        public void UpdateStudio(DataBaseWork dataBase)
        {
            dataBase.UpdateStudio(Studio);
        }
        public void DeleteStudio(DataBaseWork dataBase)
        {
            dataBase.DeleteStudio(Studio);
        }
    }
}
