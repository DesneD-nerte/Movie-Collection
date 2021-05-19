using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Studio
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<Movie> Movies { get;  set; }

        public Studio(int id, string name, string country)
        {
            ID = id;
            Name = name;
            Country = country;
            Movies = new List<Movie>();
        }
    }
}
