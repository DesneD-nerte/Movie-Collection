using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    public class Genre
    {
        public int ID { get; set; } 
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }

        public Genre(int id, string name)
        {
            ID = id;
            Name = name;
            Movies = new List<Movie>();
        }

    }
}
