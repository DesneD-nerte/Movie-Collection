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
    public class ActorViewModel : WorkspaceViewModel
    {
        internal Actor Actor { get; set; }
        MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public ActorViewModel(Actor newActor, MainWindowViewModel mainWindowViewModel = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            Actor = newActor;

            if(Actor.Gender == "Муж")
            {
                MaleGender = true;
            }
            else
            {
                WomanGender = true;
            }
            Movies = new ObservableCollection<MovieViewModel>((from movie in newActor.Movies select new MovieViewModel(movie)));
        }
        public ActorViewModel()
        {
            Actor = new Actor();
            Movies = new ObservableCollection<MovieViewModel>();
        }

        public string Name
        {
            get
            {
                return Actor.Name;
            }
            set
            {
                Actor.Name = value;
            }
        }
        public string Surname
        {
            get
            {
                return Actor.Surname;
            }
            set
            {
                Actor.Surname = value;
            }
        }
        public string Patronym
        {
            get
            {
                return Actor.Patronym;
            }
            set
            {
                Actor.Patronym = value;
            }
        }
        public string Gender
        {
            get
            {
                return Actor.Gender;
            }
            set
            {
                Actor.Gender = value;
            }
        }
        public DateTime? Birthday
        {
            get
            {
                return Actor.Birthday;
            }
            set
            {
                Actor.Birthday = value;
            }
        }
        public string Country////////////////////////
        {
            get
            {
                return Actor.Country.Name;
            }
            set
            {
                Actor.Country.Name = value;
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
            Actor.Gender = gender;
        }
        public void UpdateCountry(Country country)
        {
            Country = country.Name;
            Actor.Country = country;
        }

        RelayCommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(param => mainWindowViewModel.ShowEditActor(this));
                }
                return editCommand;
            }
        }

        public void AddActor(DataBaseWork dataBase)
        {
            if (Actor.ID == 0)
            {
                dataBase.AddActor(Actor);
            }
            else
            {
                dataBase.UpdateActor(Actor);
            }
        }
        public void UpdateActor(DataBaseWork dataBase)
        {
            dataBase.UpdateActor(Actor);
        }
        public void DeleteActor(DataBaseWork dataBase)
        {
            dataBase.DeleteActor(Actor);
        }
    }
}
