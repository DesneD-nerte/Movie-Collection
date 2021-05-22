using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Movie
    {
        /////////////////////////////////////////////////////////Раньше сеттеры были приватными
        #region Приватные сеттеры
        //public int ID { get; private set; }
        //public string Name { get; private set; }
        //public Storage Storage { get; private set; }
        //public string Description { get; private set; } 
        //public string Duration { get; private set; }  //////
        //public int CountOfSeries { get; private set; }
        //public string Release { get; private set; }//////
        //public List<Actor> Actors { get; private set; }
        //public List<Director> Directors { get; private set; }
        //public List<Genre> Genres { get; private set; } 
        //public List<Studio> Studios { get; private set; }
        #endregion

        public int ID { get; set; }
        public string Name { get; set; }
        public Storage Storage { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }  //////Были DateTime
        public int CountOfSeries { get; set; }
        public DateTime Release { get; set; }//////Были DateTime
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }
        public List<Studio> Studios { get; set; }
        public List<Genre> Genres { get; set; }

        public Movie() 
        {
            Actors = new List<Actor>();
            Directors = new List<Director>();
            Genres = new List<Genre>();
            Studios = new List<Studio>();
        }
        public Movie(int id, string name, string description, Storage storage, int numOfSeries, TimeSpan duration, DateTime release)
        {
            ID = id;
            Name = name;
            Description = description;
            Storage = storage;
            CountOfSeries = numOfSeries;
            Duration = duration;
            Release = release;
            Actors = new List<Actor>();
            Directors = new List<Director>();
            Genres = new List<Genre>();
            Studios = new List<Studio>();
        }
    }
}
