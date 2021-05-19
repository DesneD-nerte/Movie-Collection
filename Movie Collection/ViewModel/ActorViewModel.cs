using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class ActorViewModel : WorkspaceViewModel
    {
        readonly Actor actor;
        bool isSelected;

        public List<Movie> Movies { get; private set; }
        public ActorViewModel(Actor newActor)
        {
            actor = newActor;
            Movies = newActor.Movies;
        }

        public string Name
        {
            get
            {
                return actor.Name;
            }
            set
            {
                actor.Name = value;
            }
        }
        public string Surname
        {
            get
            {
                return actor.Surname;
            }
            set
            {
                actor.Surname = value;
            }
        }
        public string Patronym
        {
            get
            {
                return actor.Patronym;
            }
            set
            {
                actor.Patronym = value;
            }
        }
        public string Gender
        {
            get
            {
                return actor.Gender;
            }
            set
            {
                actor.Gender = value;
            }
        }
        public string Birthday
        {
            get
            {
                return actor.Birthday;
            }
            set
            {
                actor.Birthday = value;
            }
        }
        public string Country
        {
            get
            {
                return actor.Country;
            }
            set
            {
                actor.Country = value;
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
