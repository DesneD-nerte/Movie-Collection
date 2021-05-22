using Movie_Collection.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Actor
    {
        public int ID { get; set; }
        public string Name { get;set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Country { get; set; }
        public List<Movie> Movies { get; set; }

        public Actor(int id, string name, string surname, string patronym, string gender, string birthday, string country)
        {
            ID = id;
            Name = name;
            Surname = surname;
            Patronym = patronym;
            Gender = gender;
            Birthday = birthday;
            Country = country;
            Movies = new List<Movie>();
        }

        //public Actor(ActorViewModel actorViewModel)
        //{
        //    Name = actorViewModel.Name;
        //    Surname = actorViewModel.Surname;
        //    Patronym = Patronymic;
        //    Gender = Gender;
        //    Birthday = birthday;
        //    Country = country;
        //    Movies = new List<Movie>();
        //}

    }
}
