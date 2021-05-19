using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class DirectorViewModel : WorkspaceViewModel
    {
        readonly Director director;
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public DirectorViewModel(Director newDirector)
        {
            director = newDirector;
            Movies = newDirector.Movies;
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
        public string Birthday
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
                return director.Country;
            }
            set
            {
                director.Country = value;
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
