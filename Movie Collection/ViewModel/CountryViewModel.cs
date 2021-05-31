using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Movie_Collection.ViewModel
{
    public class CountryViewModel : WorkspaceViewModel
    {
        internal Country Country{ get; set; }

        public string Name
        {
            get
            {
                return Country.Name;
            }
            set
            {
                Country.Name = value;
            }
        }

        public ObservableCollection<StudioViewModel> Studios { get; private set; }
        public ObservableCollection<ActorViewModel> Actors { get; private set; }
        public ObservableCollection<DirectorViewModel> Directors { get; private set; }


        public CountryViewModel(Country newCountry)
        {
            Country = newCountry;
            Studios = new ObservableCollection<StudioViewModel>((from studio in newCountry.Studios select new StudioViewModel(studio)));
            Actors = new ObservableCollection<ActorViewModel>((from actor in newCountry.Actors select new ActorViewModel(actor)));
            Directors = new ObservableCollection<DirectorViewModel>((from director in newCountry.Directors select new DirectorViewModel(director)));
        }
        public CountryViewModel()
        {
            Country = new Country();
            Studios = new ObservableCollection<StudioViewModel>();
            Actors = new ObservableCollection<ActorViewModel>();
            Directors = new ObservableCollection<DirectorViewModel>();
        }

    }
}
