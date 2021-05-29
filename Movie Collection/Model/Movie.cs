using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Movie_Collection.Model
{
    class Movie : IDataErrorInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Storage Storage { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int CountOfSeries { get; set; }
        public DateTime Release { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }
        public List<Studio> Studios { get; set; }
        public List<Genre> Genres { get; set; }

        public string Error
        {
            get => throw new NotImplementedException();
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "ID":
                        if(ID < 0)
                        {
                            error = "Идентификатор не может быть отрицательным";
                        }
                        break;
                    case "Name":
                        if(String.IsNullOrEmpty(Name) || String.IsNullOrWhiteSpace(Name))
                        {
                            error = "Недопустимое название";
                        }
                        break;
                    case "CountOfSeries":
                        if(CountOfSeries < 0)
                        {
                            error = "Количество серий не может быть отрицательным";
                        }
                        break;
                }
                return error;
            }
        }

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
