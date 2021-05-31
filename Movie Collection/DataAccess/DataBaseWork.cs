using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Movie_Collection.DataAccess
{
    public class DataBaseWork
    {
        readonly string databaseConnectionString;
        public DataBaseWork(string serverName)
        {
            databaseConnectionString = serverName;
        }

        readonly List<Movie> movies = new List<Movie>();
        readonly List<Actor> actors = new List<Actor>();
        readonly List<Director> directors = new List<Director>();
        readonly List<Studio> studios = new List<Studio>();
        readonly List<Genre> genres = new List<Genre>();
        readonly List<Storage> storages = new List<Storage>();
        readonly List<Country> countries = new List<Country>();

        readonly Dictionary<int, int> movie_Actors = new Dictionary<int, int>();
        readonly Dictionary<int, int> movie_Directors = new Dictionary<int, int>();
        readonly Dictionary<int, int> movie_Studios = new Dictionary<int, int>();
        readonly Dictionary<int, int> movie_Genres = new Dictionary<int, int>();

        #region Загрузка всех фильмов, и всех сущностей к каждому фильму
        private Task LoadMovies()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT M.Id_movie, M.Name, M.Description, S.Id_storage, M.Duration, M.Number_series, M.Release, S.Name as 'StorageName' " +
                                   "FROM Movie M " +
                                       "JOIN Storage S " +
                                       "ON M.Id_storage = S.Id_storage";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Movie movie = new Movie(
                            Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["Name"].ToString(), sqlDataReader["Description"].ToString(),
                            new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), null, null);

                        if(sqlDataReader["Duration"] != DBNull.Value)
                        {
                            movie.Duration= (TimeSpan?)sqlDataReader["Duration"];
                        }
                        if (sqlDataReader["Release"] != DBNull.Value)
                        {
                            movie.Release = (DateTime?)sqlDataReader["Release"];
                        }

                        LoadMovieItems(movie);

                        movies.Add(movie);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }

            });
        }

        private void LoadMovieItems(Movie movie)
        {
            var Task1 = LoadMovieActors(movie);
            var Task2 = LoadMovieDirectors(movie);
            var Task3 = LoadMovieStudios(movie);
            var Task4 = LoadMovieGenres(movie);
            var Task5 = LoadMovieStorages(movie);

            Task.WaitAll(Task1, Task2, Task3, Task4, Task5);
        }

        private Task LoadMovieActors(Movie movie)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Actor.Id_actor, Actor.Name, Actor.Surname, Actor.Patronym, Actor.Gender, Actor.Birthday, Country.Id_country, Country.Name AS 'CountryName' " +
                                    "FROM Movie " +
                                        "JOIN Movie_Actor " +
                                        "ON Movie.Id_movie = Movie_Actor.Id_movie " +
                                        "LEFT JOIN Actor " +
                                        "ON Actor.Id_actor = Movie_Actor.Id_actor " +
                                        "LEFT JOIN Country " +
                                        "ON Actor.Id_country = Country.Id_country " +
                                    $"WHERE Movie.[Name] = '{movie.Name}' AND Movie.Id_movie = '{movie.ID}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Actor actor = new Actor(
                                Convert.ToInt32(sqlDataReader["Id_actor"]), sqlDataReader["Name"].ToString(), sqlDataReader["Surname"].ToString(),
                                sqlDataReader["Patronym"].ToString(), sqlDataReader["Gender"].ToString(), null, new Country());

                        if (sqlDataReader["Birthday"] != DBNull.Value)
                        {
                            actor.Birthday = (DateTime?)sqlDataReader["Birthday"];
                        }
                        if (sqlDataReader["Id_country"] != DBNull.Value && sqlDataReader["CountryName"] != DBNull.Value)
                        {
                            actor.Country = new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString());
                        }

                        movie.Actors.Add(actor);
                    }

                    sqlConnection.Close();
                }
            });
        }

        private Task LoadMovieDirectors(Movie movie)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Director.Id_director, Director.Name, Director.Surname, Director.Patronym, Director.Gender, Director.Birthday, Country.Id_country, Country.Name  AS 'CountryName'" +
                                   "FROM Movie " +
                                       "JOIN Movie_Director " +
                                       "ON Movie.Id_movie = Movie_Director.Id_movie " +
                                       "LEFT JOIN Director " +
                                       "ON Director.Id_director = Movie_Director.Id_director " +
                                       "LEFT JOIN Country " +
                                       "ON Director.Id_country = Country.Id_country " +
                                   $"WHERE Movie.[Name] = '{movie.Name}' AND Movie.Id_movie = '{movie.ID}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Director director = new Director(
                            Convert.ToInt32(sqlDataReader["Id_director"]), sqlDataReader["Name"].ToString(), sqlDataReader["Surname"].ToString(),
                            sqlDataReader["Patronym"].ToString(), sqlDataReader["Gender"].ToString(), null, new Country());

                        if (sqlDataReader["Birthday"] != DBNull.Value)
                        {
                            director.Birthday = (DateTime?)sqlDataReader["Birthday"];
                        }
                        if (sqlDataReader["Id_country"] != DBNull.Value && sqlDataReader["CountryName"] != DBNull.Value)
                        {
                            director.Country = new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString());
                        }

                        movie.Directors.Add(director);
                    }

                    sqlConnection.Close();
                }
            });
        }

        private Task LoadMovieStudios(Movie movie)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Studio.Id_studio, Studio.Name as 'StudioName', Country.Id_country, Country.Name as 'CountryName' " +
                                   "FROM Movie " +
                                       "JOIN Movie_Studio " +
                                       "ON Movie.Id_movie = Movie_Studio.Id_movie " +
                                       "JOIN Studio " +
                                       "ON Studio.Id_studio = Movie_Studio.Id_studio " +
                                       "JOIN Country " +
                                       "ON Studio.Id_country = Country.Id_country " +
                                   $"WHERE Movie.[Name] = '{movie.Name}' AND Movie.Id_movie = '{movie.ID}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        movie.Studios.Add(new Studio
                                            (Convert.ToInt32(sqlDataReader["Id_studio"]), sqlDataReader["StudioName"].ToString(), new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString())));
                    }

                    sqlConnection.Close();
                }
            });
        }

        private Task LoadMovieGenres(Movie movie)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Genre.Id_genre, Genre.Name " +
                                   "FROM Movie " +
                                       "JOIN Movie_Genre " +
                                       "ON Movie.Id_movie = Movie_Genre.Id_movie " +
                                       "JOIN Genre " +
                                       "ON Movie_Genre.Id_genre = Genre.Id_genre " +
                                   $"WHERE Movie.[Name] = '{movie.Name}' AND Movie.Id_movie = '{movie.ID}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        movie.Genres.Add(new Genre
                            (Convert.ToInt32(sqlDataReader["Id_genre"]), sqlDataReader["Name"].ToString()));

                    }

                    sqlConnection.Close();
                }
            });
        }
        private Task LoadMovieStorages(Movie movie)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Storage.Id_storage, Storage.Name " +
                                   "FROM Movie " +
                                        "JOIN Storage " +
                                        "ON Movie.Id_storage = Storage.Id_storage " +
                                   $"WHERE Movie.[Name] = '{movie.Name}' AND Movie.Id_movie = '{movie.ID}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        movie.Storage = new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["Name"].ToString());

                    }

                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Загрузка всех актеров и фильмов для каждого актера
        private Task LoadActors()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Actor.Id_actor, Actor.Name as 'ActorName', Actor.Surname, Actor.Patronym, Actor.Gender, Actor.Birthday, Country.Id_country, Country.Name as 'CountryName' " +
                                   "FROM Actor " +
                                       "LEFT JOIN Country " +
                                       "ON Actor.Id_country = Country.Id_country";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Actor actor = new Actor(
                               Convert.ToInt32(sqlDataReader["id_actor"]), sqlDataReader["ActorName"].ToString(), sqlDataReader["Surname"].ToString(),
                               sqlDataReader["Patronym"].ToString(), sqlDataReader["Gender"].ToString(), null, new Country());

                        if (sqlDataReader["Birthday"] != DBNull.Value)
                        {
                            actor.Birthday = (DateTime?)sqlDataReader["Birthday"];
                        }
                        if (sqlDataReader["Id_country"] != DBNull.Value && sqlDataReader["CountryName"] != DBNull.Value)
                        {
                            actor.Country = new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString());
                        }

                        LoadActorMovies(actor).Wait();

                        actors.Add(actor);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }

        private Task LoadActorMovies(Actor actor)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName' " +
                                   "FROM Movie " +
                                       "JOIN Movie_Actor " +
                                       "ON Movie_Actor.Id_movie = Movie.Id_movie " +
                                       "JOIN Actor " +
                                       "ON Actor.Id_actor = Movie_Actor.Id_actor " +
                                       "JOIN Storage " +
                                       "ON Storage.Id_storage = Movie.Id_storage " +
                                   $"Where Actor.Name = '{actor.Name}' AND Actor.Surname = '{actor.Surname}' " +
                                   $"AND(Actor.Patronym IS NULL OR Actor.Patronym = '{actor.Patronym}')";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Movie movie = new Movie(
                                Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                                new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), null, null);

                        if (sqlDataReader["Duration"] != DBNull.Value)
                        {
                            movie.Duration = (TimeSpan?)sqlDataReader["Duration"];
                        }
                        if (sqlDataReader["Release"] != DBNull.Value)
                        {
                            movie.Release = (DateTime?)sqlDataReader["Release"];
                        }

                        actor.Movies.Add(movie);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Загрузка всех Режиссеров и фильмов для каждого режиссера
        private Task LoadDirectors()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Director.Id_director, Director.Name as 'DirectorName', Director.Surname, Director.Patronym, Director.Gender, Director.Birthday, Country.Id_Country, Country.Name as 'CountryName' " +
                                   "FROM Director " +
                                       "LEFT JOIN Country " +
                                       "ON Director.Id_country = Country.Id_country";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Director director = new Director(
                               Convert.ToInt32(sqlDataReader["id_director"]), sqlDataReader["DirectorName"].ToString(), sqlDataReader["Surname"].ToString(),
                               sqlDataReader["Patronym"].ToString(), sqlDataReader["Gender"].ToString(), null, new Country());

                        if (sqlDataReader["Birthday"] != DBNull.Value)
                        {
                            director.Birthday = (DateTime?)sqlDataReader["Birthday"];
                        }
                        if (sqlDataReader["Id_country"] != DBNull.Value && sqlDataReader["CountryName"] != DBNull.Value)
                        {
                            director.Country = new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString());
                        }


                        LoadDirectorMovies(director).Wait();

                        directors.Add(director);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }

        private Task LoadDirectorMovies(Director director)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName' " +
                                   "FROM Movie " +
                                       "JOIN Movie_Director " +
                                       "ON Movie_Director.Id_movie = Movie.Id_movie " +
                                       "JOIN Director " +
                                       "ON Director.Id_director = Movie_Director.Id_director " +
                                       "JOIN Storage " +
                                       "ON Storage.Id_storage = Movie.Id_storage " +
                                   $"Where Director.Name = '{director.Name}' AND Director.Surname = '{director.Surname}' " +
                                   $"AND(Director.Patronym IS NULL OR Director.Patronym = '{director.Patronym}')";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Movie movie = new Movie(
                                Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                                new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), null, null);

                        if (sqlDataReader["Duration"] != DBNull.Value)
                        {
                            movie.Duration = (TimeSpan?)sqlDataReader["Duration"];
                        }
                        if (sqlDataReader["Release"] != DBNull.Value)
                        {
                            movie.Release = (DateTime?)sqlDataReader["Release"];
                        }

                        director.Movies.Add(movie);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion
        
        #region Загрузка всех жанров и фильмов для всех жанра
        private Task LoadGenres()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT * " +
                                   "FROM Genre ";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Genre genre;

                        genre = new Genre(
                               Convert.ToInt32(sqlDataReader["id_genre"]), sqlDataReader["Name"].ToString());


                        LoadGenreMovies(genre).Wait();

                        genres.Add(genre);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }

        private Task LoadGenreMovies(Genre genre)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName'" +
                                   "FROM Movie " +
                                       "JOIN Movie_Genre " +
                                       "ON Movie_Genre.Id_movie = Movie.Id_movie " +
                                       "JOIN Genre " +
                                       "ON Movie_Genre.Id_genre = Genre.Id_genre " +
                                       "JOIN Storage " +
                                       "ON Storage.Id_storage = Movie.Id_storage " +
                                   $"Where Genre.Name = '{genre.Name}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Movie movie = new Movie(
                                Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                                new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), null, null);

                        if (sqlDataReader["Duration"] != DBNull.Value)
                        {
                            movie.Duration = (TimeSpan?)sqlDataReader["Duration"];
                        }
                        if (sqlDataReader["Release"] != DBNull.Value)
                        {
                            movie.Release = (DateTime?)sqlDataReader["Release"];
                        }

                        genre.Movies.Add(movie);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Загрузка всех студий и фильмов для всех студий
        private Task LoadStudios()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Studio.Id_studio, Studio.Name as 'StudioName', Country.Id_country, Country.Name as 'CountryName' " +
                                   "FROM Studio " +
                                       "JOIN Country " +
                                       "ON Country.Id_country = Studio.Id_country";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Studio studio;

                        studio = new Studio(
                               Convert.ToInt32(sqlDataReader["id_studio"]), sqlDataReader["StudioName"].ToString(), new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString()));


                        LoadStudioMovies(studio).Wait();

                        studios.Add(studio);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }

        private Task LoadStudioMovies(Studio studio)
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Id_storage, Storage.Name as 'StorageName', Movie.Duration, Movie.Number_series, Movie.Release " +
                                   "FROM Studio " +
                                       "JOIN Country " +
                                       "ON Country.Id_country = Studio.Id_country " +
                                       "JOIN Movie_Studio " +
                                       "ON Movie_Studio.Id_studio = Studio.Id_studio " +
                                       "JOIN Movie " +
                                       "ON Movie.Id_movie = Movie_Studio.Id_movie " +
                                       "JOIN Storage " +
                                       "ON Storage.Id_storage = Movie.Id_storage " +
                                  $"Where Studio.Name = '{studio.Name}'";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Movie movie = new Movie(
                                Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                                new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), null, null);

                        if (sqlDataReader["Duration"] != DBNull.Value)
                        {
                            movie.Duration = (TimeSpan?)sqlDataReader["Duration"];
                        }
                        if (sqlDataReader["Release"] != DBNull.Value)
                        {
                            movie.Release = (DateTime?)sqlDataReader["Release"];
                        }

                        studio.Movies.Add(movie);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Загрузка всех накопителей
        private Task LoadStorages()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Storage.Id_storage, Storage.Name " +
                                   "FROM Storage";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Storage storage;

                        storage = new Storage(
                               Convert.ToInt32(sqlDataReader["id_storage"]), sqlDataReader["Name"].ToString());

                        storages.Add(storage);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Загрузка всех стран
        private Task LoadCountries()
        {
            return Task.Run(() =>
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT Country.Id_country, Country.Name " +
                                   "FROM Country";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Country country;

                        country = new Country(
                               Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["Name"].ToString());

                        countries.Add(country);
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            });
        }
        #endregion

        #region Добавление, удаление, обновление фильма
        public Task AddMovie(Movie movie)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "INSERT INTO Movie " +
                                       $"VALUES ('{movie.Name}', '{movie.Description}', '{movie.Storage.ID}', '{movie.CountOfSeries}', ";

                        if (movie.Duration != null)
                        {
                            query += $"{movie.Duration}, ";
                        }
                        else
                        {
                            query += "null, ";
                        }
                        if (movie.Release != null)
                        {
                            query += $"{movie.Release}) ";
                        }
                        else
                        {
                            query += "null) ";
                        }

                        query += "SELECT TOP 1* " +
                                  "FROM Movie " +
                                  "ORDER BY Id_movie DESC";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            movie.ID = Convert.ToInt32(sqlDataReader["Id_movie"]);
                        }
                        sqlDataReader.Close();

                        FillMoviesEntities(movie, sqlConnection);

                        sqlConnection.Close();
                    }

                    Success("Добавление завершено");
                }
                catch(SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch(NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        private void FillMoviesEntities(Movie movie, SqlConnection sqlConnection)
        {
            if (movie.Actors.Count != 0)
            {
                FillMovieActors(movie, sqlConnection);
            }
            if (movie.Directors.Count != 0)
            {
                FillMovieDirectors(movie, sqlConnection);
            }
            if (movie.Studios.Count != 0)
            {
                FillMovieStudios(movie, sqlConnection);
            }
            if (movie.Genres.Count != 0)
            {
                FillMovieGenres(movie, sqlConnection);
            }
        }

        private void FillMovieActors(Movie movie, SqlConnection sqlConnection)
        {
            string queryMovie_Actor = "INSERT INTO Movie_Actor VALUES ";
            foreach (var actor in movie.Actors)
            {
                if (actor != movie.Actors.Last())
                {
                    queryMovie_Actor += $"('{movie.ID}', '{actor.ID}'), ";
                }
                else
                {
                    queryMovie_Actor += $"('{movie.ID}', '{actor.ID}')";
                }
            }

            SqlCommand sqlCommandMovie_Actor = new SqlCommand(queryMovie_Actor, sqlConnection);

            SqlDataReader sqlDataReaderMovie_Actor = sqlCommandMovie_Actor.ExecuteReader();
            sqlDataReaderMovie_Actor.Close();
        }
        private void FillMovieDirectors(Movie movie, SqlConnection sqlConnection)
        {
            string queryMovie_Director = "INSERT INTO Movie_Director VALUES ";
            foreach (var director in movie.Directors)
            {
                if (director != movie.Directors.Last())
                {
                    queryMovie_Director += $"('{movie.ID}', '{director.ID}'), ";
                }
                else
                {
                    queryMovie_Director += $"('{movie.ID}', '{director.ID}')";
                }
            }

            SqlCommand sqlCommandMovie_Director = new SqlCommand(queryMovie_Director, sqlConnection);

            SqlDataReader sqlDataReaderMovie_Director = sqlCommandMovie_Director.ExecuteReader();
            sqlDataReaderMovie_Director.Close();
        }
        private void FillMovieStudios(Movie movie, SqlConnection sqlConnection)
        {
            string queryMovie_Studio = "INSERT INTO Movie_Studio VALUES ";
            foreach (var studio in movie.Studios)
            {
                if (studio != movie.Studios.Last())
                {
                    queryMovie_Studio += $"('{movie.ID}', '{studio.ID}'), ";
                }
                else
                {
                    queryMovie_Studio += $"('{movie.ID}', '{studio.ID}')";
                }
            }

            SqlCommand sqlCommandMovie_Studio = new SqlCommand(queryMovie_Studio, sqlConnection);

            SqlDataReader sqlDataReaderMovie_Studio= sqlCommandMovie_Studio.ExecuteReader();
            sqlDataReaderMovie_Studio.Close();
        }
        private void FillMovieGenres(Movie movie, SqlConnection sqlConnection)
        {
            string queryMovie_Genre = "INSERT INTO Movie_Genre VALUES ";
            foreach (var genre in movie.Genres)
            {
                if (genre != movie.Genres.Last())
                {
                    queryMovie_Genre += $"('{movie.ID}', '{genre.ID}'), ";
                }
                else
                {
                    queryMovie_Genre += $"('{movie.ID}', '{genre.ID}')";
                }
            }

            SqlCommand sqlCommandMovie_Genre = new SqlCommand(queryMovie_Genre, sqlConnection);

            SqlDataReader sqlDataReaderMovie_Genre = sqlCommandMovie_Genre.ExecuteReader();
            sqlDataReaderMovie_Genre.Close();
        }


        public Task DeleteMovie(Movie movie)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "DELETE FROM Movie " +
                                       $"WHERE Id_movie = {movie.ID }";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();

                        DeleteMovieRelations(movie, sqlConnection);

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Удаление завершено");
                }
                catch(SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch(NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }
        private void DeleteMovieRelations(Movie movie, SqlConnection sqlConnection)
        {
            DeleteRelationMovieAndActors(movie, sqlConnection);
            DeleteRelationMovieAndDirectors(movie, sqlConnection);
            DeleteRelationMovieAndStudios(movie, sqlConnection);
            DeleteRelationMovieAndGenres(movie, sqlConnection);
        }

        private void DeleteRelationMovieAndActors(Movie movie, SqlConnection sqlConnection)
        {
            string queryDeletePreviousActors = "DELETE FROM Movie_Actor " +
                                                  $"WHERE Id_Movie = {movie.ID}";

            SqlCommand sqlCommandMovie_ActorsDelete = new SqlCommand(queryDeletePreviousActors, sqlConnection);
            SqlDataReader sqlDataReaderMovie_ActorsDelete = sqlCommandMovie_ActorsDelete.ExecuteReader();

            sqlDataReaderMovie_ActorsDelete.Close();
        }
        private void DeleteRelationMovieAndDirectors(Movie movie, SqlConnection sqlConnection)
        {
            string queryDeletePreviousDirectors = "DELETE FROM Movie_Director " +
                                                  $"WHERE Id_Movie = {movie.ID}";

            SqlCommand sqlCommandMovie_DirectorsDelete = new SqlCommand(queryDeletePreviousDirectors, sqlConnection);
            SqlDataReader sqlDataReaderMovie_DirectorsDelete = sqlCommandMovie_DirectorsDelete.ExecuteReader();

            sqlDataReaderMovie_DirectorsDelete.Close();
        }
        private void DeleteRelationMovieAndStudios(Movie movie, SqlConnection sqlConnection)
        {
            string queryDeletePreviousStudios = "DELETE FROM Movie_Studio " +
                                                  $"WHERE Id_Movie = {movie.ID}";

            SqlCommand sqlCommandMovie_StudiosDelete = new SqlCommand(queryDeletePreviousStudios, sqlConnection);
            SqlDataReader sqlDataReaderMovie_StudiosDelete = sqlCommandMovie_StudiosDelete.ExecuteReader();

            sqlDataReaderMovie_StudiosDelete.Close();
        }
        private void DeleteRelationMovieAndGenres(Movie movie, SqlConnection sqlConnection)
        {
            string queryDeletePreviousGenres = "DELETE FROM Movie_Genre " +
                                                  $"WHERE Id_Movie = {movie.ID}";

            SqlCommand sqlCommandMovie_GenresDelete = new SqlCommand(queryDeletePreviousGenres, sqlConnection);
            SqlDataReader sqlDataReaderMovie_GenresDelete = sqlCommandMovie_GenresDelete.ExecuteReader();

            sqlDataReaderMovie_GenresDelete.Close();
        }

        public Task UpdateMovie(Movie movie)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "UPDATE Movie " +
                                       $"SET Name = '{movie.Name}', Description = '{movie.Description}', Id_storage = '{movie.Storage.ID}', Number_series = '{movie.CountOfSeries}', ";

                        if (movie.Duration != null)
                        {
                            query += $"Duration = '{movie.Duration}', ";
                        }
                        else
                        {
                            query += "Duration = null, ";
                        }
                        if (movie.Release != null)
                        {
                            query += $"Release = '{movie.Release}' ";
                        }
                        else
                        {
                            query += "Release = null ";
                        }

                        query += $"WHERE Id_movie = {movie.ID}";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();

                        UpdateMoviesEntities(movie, sqlConnection);

                        sqlConnection.Close();
                    }

                    Success("Изменение завершено");
                }
                catch(SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch(NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch(Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        private void UpdateMoviesEntities(Movie movie, SqlConnection sqlConnection)
        {
            UpdateMovieActors(movie, sqlConnection);
            UpdateMovieDirectors(movie, sqlConnection);
            UpdateMovieStudios(movie, sqlConnection);
            UpdateMovieGenres(movie, sqlConnection);
        }

        private void UpdateMovieActors(Movie movie, SqlConnection sqlConnection)
        {
            DeleteRelationMovieAndActors(movie, sqlConnection);

            if (movie.Actors.Count != 0)
            {
                FillMovieActors(movie, sqlConnection);
            }
        }
        private void UpdateMovieDirectors(Movie movie, SqlConnection sqlConnection)
        {
            DeleteRelationMovieAndDirectors(movie, sqlConnection);

            if (movie.Directors.Count != 0)
            {
                FillMovieDirectors(movie, sqlConnection);
            }
        }
        private void UpdateMovieStudios(Movie movie, SqlConnection sqlConnection)
        {
            DeleteRelationMovieAndStudios(movie, sqlConnection);

            if (movie.Studios.Count != 0)
            {
                FillMovieStudios(movie, sqlConnection);
            }
        }
        private void UpdateMovieGenres(Movie movie, SqlConnection sqlConnection)
        {
            DeleteRelationMovieAndGenres(movie, sqlConnection);

            if (movie.Genres.Count != 0)
            {
                FillMovieGenres(movie, sqlConnection);
            }
        }
        #endregion

        #region Добавление, удаление, обновление актера
        public Task AddActor(Actor actor)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "INSERT INTO Actor " +
                                       $"VALUES ('{actor.Name}', '{actor.Surname}', '{actor.Patronym}', '{actor.Gender}', '{actor.Birthday}', '{actor.Country.ID}')";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Добавление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task DeleteActor(Actor actor)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "DELETE FROM Actor " +
                                       $"WHERE Id_Actor = {actor.ID }";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Удаление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task UpdateActor(Actor actor)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "UPDATE Actor " +
                                       $"SET Name = '{actor.Name}', Surname = '{actor.Surname}', Patronym = '{actor.Patronym}', Gender = '{actor.Gender}', Birthday = '{actor.Birthday}', Id_country = '{actor.Country.ID}' " +
                                       $"WHERE Id_actor = {actor.ID}";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Изменение завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }
        #endregion

        #region Добавление, удаление, обновление режиссера
        public Task AddDirector(Director director)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "INSERT INTO Director " +
                                       $"VALUES ('{director.Name}', '{director.Surname}', '{director.Patronym}', '{director.Gender}', '{director.Birthday}', '{director.Country.ID}')";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Добавление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task DeleteDirector(Director director)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "DELETE FROM Director " +
                                       $"WHERE Id_director = {director.ID }";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Удаление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task UpdateDirector(Director director)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "UPDATE Director " +
                                       $"SET Name = '{director.Name}', Surname = '{director.Surname}', Patronym = '{director.Patronym}', Gender = '{director.Gender}', Birthday = '{director.Birthday}', Id_country = '{director.Country.ID}' " +
                                       $"WHERE Id_director = {director.ID}";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Изменение завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }
        #endregion

        #region Удаление жанра
        public Task DeleteGenre(Genre genre)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "DELETE FROM Genre " +
                                       $"WHERE Id_genre = {genre.ID}";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Удаление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        #endregion

        #region Добавление, удаление, обновление студии
        public Task AddStudio(Studio studio)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "INSERT INTO Studio " +
                                       $"VALUES ('{studio.Name}', '{studio.Country.ID}')";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Добавление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task DeleteStudio(Studio studio)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "DELETE FROM Studio " +
                                       $"WHERE Id_studio = {studio.ID }";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Удаление завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }

        public Task UpdateStudio(Studio studio)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                    {
                        string query = "UPDATE Studio " +
                                       $"SET Name = '{studio.Name}', Id_country = '{studio.Country.ID}'" +
                                       $"WHERE Id_studio = {studio.ID}";

                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }

                    Success("Изменение завершено");
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    NullArgumentsDataBase(ex.Message);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);
                }
            });
        }
        #endregion

        #region Загрузка связей фильма и его сущностей
        private void LoadRelationMoviesAndActors()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
                {
                    string query = "SELECT * " +
                                   "FROM Movie_Actor";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        movie_Actors.Add(Convert.ToInt32(sqlDataReader["Id_movie"]), Convert.ToInt32(sqlDataReader["Id_actor"]));
                    }

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
        }

        private void LoadRelationMoviesAndDirectors()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT * " +
                               "FROM Movie_Director";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    movie_Directors.Add(Convert.ToInt32(sqlDataReader["Id_movie"]), Convert.ToInt32(sqlDataReader["Id_director"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadRelationMoviesAndStudios()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT * " +
                               "FROM Movie_Studio";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    movie_Studios.Add(Convert.ToInt32(sqlDataReader["Id_movie"]), Convert.ToInt32(sqlDataReader["Id_studio"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadRelationMoviesAndGenres()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT * " +
                               "FROM Movie_genre";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    movie_Genres.Add(Convert.ToInt32(sqlDataReader["Id_movie"]), Convert.ToInt32(sqlDataReader["Id_genre"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        public event EventHandler<ErrorEventArgs> ErrorMessage;

        #region Получение сущностей
        public Task<List<Movie>> GetMovies()
        {
            return Task.Run<List<Movie>>(async () =>
            {
                try
                {
                    movies.Clear();

                    await LoadMovies();

                    Success("Загрузка завершена");

                    return new List<Movie>(movies);
                }
                catch (SqlException ex) 
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Movie>(movies);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Movie>(movies);
                }
            });
        }
        public Task<List<Actor>> GetActors()
        {
            return Task.Run<List<Actor>>(async() =>
            {
                try
                {
                    actors.Clear();

                    await LoadActors();

                    Success("Загрузка завершена");

                    return new List<Actor>(actors);
                }
                catch(SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Actor>(actors);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Actor>(actors);
                }
            });
        }
        public Task<List<Director>> GetDirectors()
        {
            return Task.Run<List<Director>>(async () =>
            {
                try
                {
                    directors.Clear();

                    await LoadDirectors();

                    Success("Загрузка завершена");

                    return new List<Director>(directors);
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Director>(directors);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Director>(directors);
                }
            });
        }
        public Task<List<Studio>> GetStudios()
        {
            return Task.Run<List<Studio>>(async () =>
            {
                try
                {
                    studios.Clear();

                    await LoadStudios();

                    Success("Загрузка завершена");

                    return new List<Studio>(studios);
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Studio>(studios);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Studio>(studios);
                }
            });
        }
        public Task<List<Genre>> GetGenres()
        {
            return Task.Run<List<Genre>>(async () =>
            {
                try
                {
                    genres.Clear();

                    await LoadGenres();

                    Success("Загрузка завершена");

                    return new List<Genre>(genres);
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Genre>(genres);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Genre>(genres);
                }
            });
        }
        public Task<List<Storage>> GetStorages()
        {
            return Task.Run<List<Storage>>(async () =>
            {
                try
                {
                    storages.Clear();

                    await LoadStorages();

                    Success("Загрузка завершена");

                    return new List<Storage>(storages);
                }
                catch(SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Storage>(storages);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Storage>(storages);
                }
            });
        }
        public Task<List<Country>> GetCountries()
        {
            return Task.Run<List<Country>>(async () =>
            {
                try
                {
                    countries.Clear();

                    await LoadCountries();

                    Success("Загрузка завершена");

                    return new List<Country>(countries);
                }
                catch (SqlException ex)
                {
                    NoConnectionToDataBase(ex.Message);

                    return new List<Country>(countries);
                }
                catch (Exception ex)
                {
                    UnexpectedException(ex.Message);

                    return new List<Country>(countries);
                }
            });
        }

        private void GetMovie_Actors()
        {
            movie_Actors.Clear();

            LoadRelationMoviesAndActors();
        }
        private void GetMovie_Directors()
        {
            movie_Directors.Clear();

            LoadRelationMoviesAndDirectors();
        }
        private void GetMovie_Studios()
        {
            movie_Studios.Clear();

            LoadRelationMoviesAndStudios();
        }
        private void GetMovie_Genres()
        {
            movie_Genres.Clear();

            LoadRelationMoviesAndGenres();
        }
        #endregion

        void NoConnectionToDataBase(string exceptionMessage)
        {
            ErrorMessage(this, new ErrorEventArgs("Отcутствует подключение к базе данных или ошибка при работе с ней: \n" + exceptionMessage));
        }
        void NullArgumentsDataBase(string exceptionMessage)
        {
            ErrorMessage(this, new ErrorEventArgs("Ошибка при передаче параметров в базу данных: \n" + exceptionMessage));
        }
        void UnexpectedException(string exceptionMessage)
        {
            ErrorMessage(this, new ErrorEventArgs("Неизвестная ошибка: \n" + exceptionMessage));
        }
        void Success(string Message)
        {
            ErrorMessage(this, new ErrorEventArgs(Message));
        }
    }
}
