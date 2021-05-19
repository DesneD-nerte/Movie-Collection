using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class StudioViewModel : WorkspaceViewModel
    {
        readonly Studio studio;
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public StudioViewModel(Studio newStudio)
        {
            studio = newStudio;
            Movies = newStudio.Movies;
        }

        public string Name
        {
            get
            {
                return studio.Name;
            }
            set
            {
                studio.Name = value;
            }
        }
        public string Country
        {
            get
            {
                return studio.Country;
            }
            set
            {
                studio.Country = value;
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
