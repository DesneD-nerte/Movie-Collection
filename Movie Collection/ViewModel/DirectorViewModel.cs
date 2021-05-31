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
    public class DirectorViewModel : WorkspaceViewModel
    {
        internal Director Director { get; set; }
        MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public DirectorViewModel(Director newDirector, MainWindowViewModel mainWindowViewModel = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            Director = newDirector;

            if (Director.Gender == "Муж")
            {
                MaleGender = true;
            }
            else
            {
                WomanGender = true;
            }

            Movies = new ObservableCollection<MovieViewModel>((from movie in newDirector.Movies select new MovieViewModel(movie)));
        }

        public DirectorViewModel()
        {
            Director = new Director();
            Movies = new ObservableCollection<MovieViewModel>();
        }

        public string Name
        {
            get
            {
                return Director.Name;
            }
            set
            {
                Director.Name = value;
            }
        }
        public string Surname
        {
            get
            {
                return Director.Surname;
            }
            set
            {
                Director.Surname = value;
            }
        }
        public string Patronym
        {
            get
            {
                return Director.Patronym;
            }
            set
            {
                Director.Patronym = value;
            }
        }
        public string Gender
        {
            get
            {
                return Director.Gender;
            }
            set
            {
                Director.Gender = value;
            }
        }
        public DateTime? Birthday
        {
            get
            {
                return Director.Birthday;
            }
            set
            {
                Director.Birthday = value;
            }
        }
        public string Country
        {
            get
            {
                return Director.Country.Name;
            }
            set
            {
                Director.Country.Name = value;
            }
        }

        private bool maleGender;
        private bool womanGender;

        public bool MaleGender
        {
            get
            {
                return maleGender;
            }
            set
            {
                if (value == true)
                {
                    maleGender = true;
                    WomanGender = false;
                }
                else
                {
                    maleGender = false;
                }
                Gender = "Муж";
                base.OnPropertyChanged("MaleGender");
            }
        }
        public bool WomanGender
        {
            get
            {
                return womanGender;
            }
            set
            {
                if (value == true)
                {
                    womanGender = true;
                    MaleGender = false;
                }
                else
                {
                    womanGender = false;
                }
                Gender = "Жен";
                base.OnPropertyChanged("WomanGender");
            }
        }

        public void UpdateGender(string gender)
        {
            Gender = gender;
            Director.Gender = gender;
        }
        public void UpdateCountry(Country country)
        {
            Country = country.Name;
            Director.Country = country;
        }

        RelayCommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(param => mainWindowViewModel.ShowEditDirector(this));
                }
                return editCommand;
            }
        }

        public void AddDirector(DataBaseWork dataBase)
        {
            if (Director.ID == 0)
            {
                dataBase.AddDirector(Director);
            }
            else
            {
                dataBase.UpdateDirector(Director);
            }
        }
        public void UpdateDirector(DataBaseWork dataBase)
        {
            dataBase.UpdateDirector(Director);
        }
        public void DeleteDirector(DataBaseWork dataBase)
        {
            dataBase.DeleteDirector(Director);
        }
    }
}
