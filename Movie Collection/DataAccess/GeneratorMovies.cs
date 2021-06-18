using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Bogus.DataSets;

namespace Movie_Collection.DataAccess
{
    class GeneratorMovies
    {
        Faker faker = new Faker("ru");

        public List<Movie> Movies;
        public List<Actor> Actors;
        public List<Director> Directors;
        public List<Studio> Studios;
        public List<Genre> Genres;
        public async void CreateNewMovies(DataBaseWork dataBaseWork, int countMovies)
        {
            SetExistingEntities(dataBaseWork);

            Faker<Movie> generatorMovie = GetGeneratorMovie();

            Movies = new List<Movie>(generatorMovie.Generate(countMovies));

            foreach (var movie in Movies)
            {
               await dataBaseWork.AddMovie(movie);
            }
        }

        private Faker<Movie> GetGeneratorMovie()
        {
            return new Faker<Movie>("ru")
                .RuleFor(x => x.Name, f => f.Hacker.Noun())
                .RuleFor(x => x.Description, f => f.Lorem.Text())
                .RuleFor(x => x.Storage, f =>
                {
                    return new Storage
                    {
                        ID = f.Random.Number(1, 5)
                        //Name = f.Random.ListItem(new List<string> {
                        //    "Blu-ray Disc",
                        //    "DVD",
                        //    "Флеш-память",
                        //    "HDD",
                        //    "SSD"
                        //})
                    };
                })
                .RuleFor(x => x.Duration, f => null)
                .RuleFor(x => x.CountOfSeries, f => f.Random.Number(1, 10))
                                //.RuleFor(x => x.Release, f => f.Date.Between(Convert.ToDateTime("2000.01.01"), Convert.ToDateTime("2010.01.01")))
                .RuleFor(x => x.Release, f => null)
                .RuleFor(x => x.Actors, f =>
                {
                    return new List<Actor>
                    {
                        f.PickRandom(Actors)
                    };
                })
                .RuleFor(x => x.Directors, f =>
                {
                    return new List<Director>
                    {
                        f.PickRandom(Directors)
                    };
                })
                .RuleFor(x => x.Studios, f =>
                {
                    return new List<Studio>
                    {
                        f.PickRandom(Studios)
                    };
                })
                .RuleFor(x => x.Genres, f =>
                {
                    return new List<Genre>
                    {
                        f.PickRandom(Genres)
                    };
                });
        }

        private void SetExistingEntities(DataBaseWork dataBaseWork)
        {
            Actors = dataBaseWork.GetActors().Result;
            Directors = dataBaseWork.GetDirectors().Result;
            Studios = dataBaseWork.GetStudios().Result;
            Genres = dataBaseWork.GetGenres().Result;
        }
    }
}
