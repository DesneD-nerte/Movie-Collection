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
        public string Duration { get; set; }  //////Были DateTime
        public int CountOfSeries { get; set; }
        public string Release { get; set; }//////Были DateTime
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Studio> Studios { get; set; }

        public Movie() { }
        public Movie(int inputID, string inputName, string inputDescription, Storage inputstorage, int inputNumSeries, string inputDuration, string inputReleased)
        {
            ID = inputID;
            Name = inputName;
            Description = inputDescription;
            Storage = inputstorage;
            CountOfSeries = inputNumSeries;
            Duration = inputDuration;
            Release = inputReleased;
            Actors = new List<Actor>();
            Directors = new List<Director>();
            Genres = new List<Genre>();
            Studios = new List<Studio>();
        }
    }
}
