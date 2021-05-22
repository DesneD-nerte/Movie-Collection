using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class GenreViewModel : WorkspaceViewModel
    {
        internal Genre Genre { get; set; }
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public GenreViewModel(Genre newGenre)
        {
            Genre = newGenre;
            Movies = newGenre.Movies;
        }

        public string Name
        {
            get
            {
                return Genre.Name;
            }
            set
            {
                Genre.Name = value;
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
