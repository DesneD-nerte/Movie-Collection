using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Movie_Collection.Model
{
    public class Movie : IDataErrorInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Storage Storage { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public int CountOfSeries { get; set; }
        public DateTime? Release { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Director> Directors { get; set; }
        public List<Studio> Studios { get; set; }
        public List<Genre> Genres { get; set; }


        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = String.Empty;

                switch (propertyName)
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
                        if(CountOfSeries < 1)
                        {
                            error = "Количество серий не может менее одного";
                        }
                        break;
                    case "Duration":
                        {
                            if (Duration.HasValue && Duration.Value < TimeSpan.Zero)
                            {
                                error = "Продолжительность задана некорректно";
                            }
                            break;
                        }
                    case "Release":
                        {
                            if (Release.HasValue && Release.Value < DateTime.MinValue)
                            {
                                error = "Дата выхода недействительная";
                            }
                            break;
                        }
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
        public Movie(int id, string name, string description, Storage storage, int numOfSeries, TimeSpan? duration, DateTime? release)
        {
            CheckParameters(id, storage, numOfSeries, duration, release);

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

        private void CheckParameters(int id, Storage storage, int numOfSeries, TimeSpan? duration, DateTime? release)
        {
            if (id < 0 || storage == null || numOfSeries < 1 || (duration.HasValue && duration.Value < TimeSpan.Zero) || (release.HasValue && release.Value < DateTime.MinValue))
            {
                throw new ArgumentException("Ошибка при передачи параметра");
            }
        }

        public bool CheckPropertiesBeforeAdding()
        {
            if (ID < 0 || Storage == null || CountOfSeries < 1 || (Duration.HasValue && Duration.Value < TimeSpan.Zero) || (Release.HasValue && Release.Value < DateTime.MinValue))
            {
                throw new ArgumentException("Ошибка при передачи параметра");
            }

            return true;
        }
    }
}
