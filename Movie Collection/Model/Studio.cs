using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Studio
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public List<Movie> Movies { get;  set; }

        public Studio(int id, string name, Country country)
        {
            ID = id;
            Name = name;
            Country = country;
            Movies = new List<Movie>();
        }
        public Studio()
        {
            Country = new Country();
            Movies = new List<Movie>();
        }
    }
}
