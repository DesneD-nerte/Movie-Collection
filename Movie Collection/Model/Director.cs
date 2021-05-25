using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Director
    {
        public int ID { get; set; } 
        public string Name { get; set; }    
        public string Surname { get;  set; } 
        public string Patronym { get;  set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public Country Country { get; set; }
        public List<Movie> Movies { get; set; }

        public Director()
        {
            Country = new Country();
            Movies = new List<Movie>();
        }

        public Director(int id, string name, string surname, string patronym, string gender, DateTime? birthday, Country country)
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

    }
}
