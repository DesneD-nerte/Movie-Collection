using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class DirectorViewModel : WorkspaceViewModel
    {
        internal Director Director { get; set; }
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public DirectorViewModel(Director newDirector)
        {
            Director = newDirector;
            Movies = newDirector.Movies;
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
        public string Birthday
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
                return Director.Country;
            }
            set
            {
                Director.Country = value;
            }
        }
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value == isSelected)
                {
                    return;
                }
                isSelected = value;
                base.OnPropertyChanged("IsSelected");
            }
        }
    }
}
