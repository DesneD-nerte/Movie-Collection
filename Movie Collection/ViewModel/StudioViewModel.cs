using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class StudioViewModel : WorkspaceViewModel
    {
        internal Studio Studio { get; set; }

        public ObservableCollection<MovieViewModel> Movies { get; private set; }
        public StudioViewModel(Studio newStudio)
        {
            Studio = newStudio;
            Movies = new ObservableCollection<MovieViewModel>((from movie in newStudio.Movies select new MovieViewModel(movie)));
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
        public string Country/////////////////////////////
        {
            get
            {
                return Studio.Country.Name;
            }
            set
            {
                Studio.Country.Name = value;
            }
        }
    }
}
