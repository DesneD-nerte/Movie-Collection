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
    class DirectorViewModel : WorkspaceViewModel
    {
        internal Director director;
        MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public DirectorViewModel(Director newDirector, MainWindowViewModel mainWindowViewModel = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            director = newDirector;

            if (director.Gender == "Муж")
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
            director = new Director();
            Movies = new ObservableCollection<MovieViewModel>();
        }

        public string Name
        {
            get
            {
                return director.Name;
            }
            set
            {
                director.Name = value;
            }
        }
        public string Surname
        {
            get
            {
                return director.Surname;
            }
            set
            {
                director.Surname = value;
            }
        }
        public string Patronym
        {
            get
            {
                return director.Patronym;
            }
            set
            {
                director.Patronym = value;
            }
        }
        public string Gender
        {
            get
            {
                return director.Gender;
            }
            set
            {
                director.Gender = value;
            }
        }
        public DateTime? Birthday
        {
            get
            {
                return director.Birthday;
            }
            set
            {
                director.Birthday = value;
            }
        }
        public string Country
        {
            get
            {
                return director.Country.Name;
            }
            set
            {
                director.Country.Name = value;
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
            director.Gender = gender;
        }
        public void UpdateCountry(Country country)
        {
            Country = country.Name;
            director.Country = country;
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
            if (director.ID == 0)
            {
                dataBase.AddDirector(director);
            }
            else
            {
                dataBase.UpdateDirector(director);
            }
        }
        public void UpdateDirector(DataBaseWork dataBase)
        {
            dataBase.UpdateDirector(director);
        }
        public void DeleteDirector(DataBaseWork dataBase)
        {
            dataBase.DeleteDirector(director);
        }
    }
}
