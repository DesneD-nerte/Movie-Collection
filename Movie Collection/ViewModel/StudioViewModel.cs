using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class StudioViewModel : WorkspaceViewModel
    {
        internal Studio Studio { get; set; }
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public StudioViewModel(Studio newStudio)
        {
            Studio = newStudio;
            Movies = newStudio.Movies;
        }

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
        public string Country
        {
            get
            {
                return Studio.Country;
            }
            set
            {
                Studio.Country = value;
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
