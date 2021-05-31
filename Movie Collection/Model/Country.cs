using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }
        public List<Studio> Studios { get; set; }
        public Country()
        {
            Actors = new List<Actor>();
            Directors = new List<Director>();
            Studios = new List<Studio>();
        }
        public Country(int id, string name)
        {
            ID = id;
            Name = name;
            Actors = new List<Actor>();
            Directors = new List<Director>();
            Studios = new List<Studio>();
        }
    }
}
