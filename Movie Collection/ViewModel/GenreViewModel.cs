using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class GenreViewModel : WorkspaceViewModel
    {
        readonly Genre genre;
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public GenreViewModel(Genre newGenre)
        {
            genre = newGenre;
            Movies = newGenre.Movies;
        }

        public string Name
        {
            get
            {
                return genre.Name;
            }
            set
            {
                genre.Name = value;
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
