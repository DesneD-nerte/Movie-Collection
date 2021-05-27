using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Movie_Collection.DataAccess
{
    class DataBaseWork
    {
        string databaseConnectionString;
        public DataBaseWork(string serverName)
        {
            databaseConnectionString = serverName;
        }

        List<Movie> movies = new List<Movie>();
        List<Actor> actors = new List<Actor>();
        List<Director> directors = new List<Director>();
        List<Studio> studios = new List<Studio>();
        List<Genre> genres = new List<Genre>();
        List<Storage> storages = new List<Storage>();
        List<Country> countries = new List<Country>();

        #region Загрузка всех фильмов, и всех сущностей к каждому фильму
        private void LoadMovies()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT M.Id_movie, M.Name, M.Description, S.Id_storage, M.Duration, M.Number_series, M.Release, S.Name as 'StorageName' "+
                               "FROM Movie M "+
                                   "JOIN Storage S "+
                                   "ON M.Id_storage = S.Id_storage";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while(sqlDataReader.Read())
                {
                    Movie movie = new Movie(
                        Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["Name"].ToString(), sqlDataReader["Description"].ToString(),
                        new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), (TimeSpan)sqlDataReader["Duration"], (DateTime)sqlDataReader["Release"]);

                    LoadMovieActors(movie);
                    LoadMovieDirectors(movie);
                    LoadMovieStudios(movie);
                    LoadMovieGenres(movie);
                    LoadMovieStorages(movie);

                    movies.Add(movie);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadMovieActors(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Actor.Id_actor, Actor.Name, Actor.Surname, Actor.Patronym, Actor.Gender, Actor.Birthday, Country.Id_country, Country.Name AS 'CountryName' "+
                                "FROM Movie "+
                                    "JOIN Movie_Actor "+
                                    "ON Movie.Id_movie = Movie_Actor.Id_movie "+
                                    "LEFT JOIN Actor "+
                                    "ON Actor.Id_actor = Movie_Actor.Id_actor "+
                                    "LEFT JOIN Country "+
                                    "ON Actor.Id_country = Country.Id_country "+
                                $"WHERE Movie.[Name] = '{movie.Name}'";

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
        }

        private void LoadMovieDirectors(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Director.Id_director, Director.Name, Director.Surname, Director.Patronym, Director.Gender, Director.Birthday, Country.Id_country, Country.Name  AS 'CountryName'" +
                               "FROM Movie "+
                                   "JOIN Movie_Director "+
                                   "ON Movie.Id_movie = Movie_Director.Id_movie "+
                                   "LEFT JOIN Director "+
                                   "ON Director.Id_director = Movie_Director.Id_director "+
                                   "LEFT JOIN Country "+
                                   "ON Director.Id_country = Country.Id_country "+
                               $"WHERE Movie.[Name] = '{movie.Name}'";

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
        }

        private void LoadMovieStudios(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Studio.Id_studio, Studio.Name as 'StudioName', Country.Id_country, Country.Name as 'CountryName' "+
                               "FROM Movie "+
                                   "JOIN Movie_Studio "+
                                   "ON Movie.Id_movie = Movie_Studio.Id_movie "+
                                   "JOIN Studio "+
                                   "ON Studio.Id_studio = Movie_Studio.Id_studio "+
                                   "JOIN Country "+
                                   "ON Studio.Id_country = Country.Id_country "+
                               $"WHERE Movie.[Name] = '{movie.Name}'";

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
        }

        private void LoadMovieGenres(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Genre.Id_genre, Genre.Name "+
                               "FROM Movie "+
                                   "JOIN Movie_Genre "+
                                   "ON Movie.Id_movie = Movie_Genre.Id_movie "+
                                   "JOIN Genre "+
                                   "ON Movie_Genre.Id_genre = Genre.Id_genre "+
                               $"WHERE Movie.[Name] = '{movie.Name}'";

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
        }
        private void LoadMovieStorages(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Storage.Id_storage, Storage.Name "+
                               "FROM Movie "+
                                    "JOIN Storage "+
                                    "ON Movie.Id_storage = Storage.Id_storage "+
                               $"WHERE Movie.[Name] = '{movie.Name}'";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    movie.Storage = new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["Name"].ToString());

                }

                sqlConnection.Close();
            }
        }
        #endregion

        #region Загрузка всех актеров и фильмов для каждого актера
        private void LoadActors()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Actor.Id_actor, Actor.Name as 'ActorName', Actor.Surname, Actor.Patronym, Actor.Gender, Actor.Birthday, Country.Id_country, Country.Name as 'CountryName' "+
                               "FROM Actor "+
                                   "LEFT JOIN Country "+
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
                    if(sqlDataReader["Id_country"] != DBNull.Value && sqlDataReader["CountryName"] != DBNull.Value)
                    {
                        actor.Country = new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString());
                    }

                    LoadActorMovies(actor);

                    actors.Add(actor);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadActorMovies(Actor actor)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName' "+
                               "FROM Movie "+
                                   "JOIN Movie_Actor "+
                                   "ON Movie_Actor.Id_movie = Movie.Id_movie "+
                                   "JOIN Actor "+
                                   "ON Actor.Id_actor = Movie_Actor.Id_actor "+
                                   "JOIN Storage "+
                                   "ON Storage.Id_storage = Movie.Id_storage "+
                               $"Where Actor.Name = '{actor.Name}' AND Actor.Surname = '{actor.Surname}' " +
                               $"AND(Actor.Patronym IS NULL OR Actor.Patronym = '{actor.Patronym}')";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {     
                    actor.Movies.Add(new Movie(
                            Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                            new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), (TimeSpan)sqlDataReader["Duration"], (DateTime)sqlDataReader["Release"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        #region Загрузка всех Режиссеров и фильмов для каждого режиссера
        private void LoadDirectors()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Director.Id_director, Director.Name as 'DirectorName', Director.Surname, Director.Patronym, Director.Gender, Director.Birthday, Country.Id_Country, Country.Name as 'CountryName' "+
                               "FROM Director "+
                                   "LEFT JOIN Country "+
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


                    LoadDirectorMovies(director);

                    directors.Add(director);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadDirectorMovies(Director director)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName' "+
                               "FROM Movie "+
                                   "JOIN Movie_Director "+
                                   "ON Movie_Director.Id_movie = Movie.Id_movie "+
                                   "JOIN Director "+
                                   "ON Director.Id_director = Movie_Director.Id_director "+
                                   "JOIN Storage "+
                                   "ON Storage.Id_storage = Movie.Id_storage "+
                               $"Where Director.Name = '{director.Name}' AND Director.Surname = '{director.Surname}' " +
                               $"AND(Director.Patronym IS NULL OR Director.Patronym = '{director.Patronym}')";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    director.Movies.Add(new Movie(
                            Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                            new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), (TimeSpan)sqlDataReader["Duration"], (DateTime)sqlDataReader["Release"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion
        
        #region Загрузка всех жанров и фильмов для всех жанра
        private void LoadGenres()
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


                    LoadGenreMovies(genre);

                    genres.Add(genre);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadGenreMovies(Genre genre)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Duration, Movie.Number_series, Movie.Release, Storage.Id_storage, Storage.Name as 'StorageName'"+
                               "FROM Movie "+
                                   "JOIN Movie_Genre "+
                                   "ON Movie_Genre.Id_movie = Movie.Id_movie "+
                                   "JOIN Genre "+
                                   "ON Movie_Genre.Id_genre = Genre.Id_genre "+
                                   "JOIN Storage "+
                                   "ON Storage.Id_storage = Movie.Id_storage "+
                               $"Where Genre.Name = '{genre.Name}'";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    //genre.Movies.Add(new Movie(
                    //    Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                    //    new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), sqlDataReader["Duration"].ToString(), sqlDataReader["Release"].ToString()));
                    genre.Movies.Add(new Movie(
                           Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                           new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), (TimeSpan)sqlDataReader["Duration"], (DateTime)sqlDataReader["Release"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        #region Загрузка всех студий и фильмов для всех студий
        private void LoadStudios()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Studio.Id_studio, Studio.Name as 'StudioName', Country.Id_country, Country.Name as 'CountryName' "+
                               "FROM Studio "+
                                   "JOIN Country "+
                                   "ON Country.Id_country = Studio.Id_country";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Studio studio;

                    studio = new Studio(
                           Convert.ToInt32(sqlDataReader["id_studio"]), sqlDataReader["StudioName"].ToString(), new Country(Convert.ToInt32(sqlDataReader["Id_country"]), sqlDataReader["CountryName"].ToString()));


                    LoadStudioMovies(studio);

                    studios.Add(studio);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        private void LoadStudioMovies(Studio studio)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Movie.Id_movie, Movie.Name as 'MovieName', Movie.Description, Movie.Id_storage, Storage.Name as 'StorageName', Movie.Duration, Movie.Number_series, Movie.Release "+
                               "FROM Studio "+
                                   "JOIN Country "+
                                   "ON Country.Id_country = Studio.Id_country "+
                                   "JOIN Movie_Studio "+
                                   "ON Movie_Studio.Id_studio = Studio.Id_studio "+
                                   "JOIN Movie "+
                                   "ON Movie.Id_movie = Movie_Studio.Id_movie "+
                                   "JOIN Storage "+
                                   "ON Storage.Id_storage = Movie.Id_storage "+
                              $"Where Studio.Name = '{studio.Name}'";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    //studio.Movies.Add(new Movie(
                    //    Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                    //    new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), sqlDataReader["Duration"].ToString(), sqlDataReader["Release"].ToString()));
                    studio.Movies.Add(new Movie(
                            Convert.ToInt32(sqlDataReader["id_movie"]), sqlDataReader["MovieName"].ToString(), sqlDataReader["Description"].ToString(),
                            new Storage(Convert.ToInt32(sqlDataReader["Id_storage"]), sqlDataReader["StorageName"].ToString()), Convert.ToInt32(sqlDataReader["Number_series"]), (TimeSpan)sqlDataReader["Duration"], (DateTime)sqlDataReader["Release"]));
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        #region Загрузка всех накопителей
        private void LoadStorages()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT Storage.Id_storage, Storage.Name "+
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
        }
        #endregion

        #region Загрузка всех стран
        private void LoadCountries()
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
        }
        #endregion

        #region Добавление, удаление, обновление фильма
        public void AddMovie(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "INSERT INTO Movie " +
                               $"VALUES ('{movie.Name}', '{movie.Description}', '{movie.Storage.ID}', '{movie.CountOfSeries}', '{movie.Duration}', '{movie.Release}')";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        public void DeleteMovie(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "DELETE FROM Movie " +
                               $"WHERE Id_movie = {movie.ID }";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();



                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        public void UpdateMovie(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "UPDATE Movie " +
                               $"SET Name = '{movie.Name}', Description = '{movie.Description}', Id_storage = '{movie.Storage.ID}', Number_series = '{movie.CountOfSeries}', Duration = '{movie.Duration}', Release = '{movie.Release}' " +
                               $"WHERE Id_movie = {movie.ID}";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        #region Добавление, удаление, обновление актера
        public void AddActor(Actor actor)
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
        }

        public void DeleteActor(Actor actor)
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
        }

        public void UpdateActor(Actor actor)
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "UPDATE Actor " +
                               $"SET Name = '{actor.Name}', Surname = '{actor.Surname}', Patronym = '{actor.Patronym}', Gender = '{actor.Gender}', Birthday = '{actor.Birthday}', Id_country = '{actor.Country.ID}' "+
                               $"WHERE Id_actor = {actor.ID}";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }
        #endregion

        #region Добавление, удаление, обновление режиссера
        public void AddDirector(Director director)
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
        }

        public void DeleteDirector(Director director)
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
        }

        public void UpdateDirector(Director director)
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
        }
        #endregion

        #region Удаление жанра
        public void DeleteGenre(Genre genre)
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
        }

        #endregion

        #region Добавление, удаление, обновление студии
        public void AddStudio(Studio studio)
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
        }

        public void DeleteStudio(Studio studio)
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
        }

        public void UpdateStudio(Studio studio)
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
        }
        #endregion

        #region Получение сущностей
        public Task<List<Movie>> GetMovies()
        {
            return Task.Run<List<Movie>>(() =>
            {
                movies.Clear();

                LoadMovies();
                return new List<Movie>(movies);
            });
        }
        public List<Actor> GetActors()
        {
            actors.Clear();

            LoadActors();
            return new List<Actor>(actors);
        }
        public List<Director> GetDirectors()
        {
            directors.Clear();

            LoadDirectors();
            return new List<Director>(directors);
        }
        public List<Studio> GetStudios()
        {
            studios.Clear();

            LoadStudios();
            return new List<Studio>(studios);
        }
        public List<Genre> GetGenres()
        {
            genres.Clear();

            LoadGenres();
            return new List<Genre>(genres);
        }
        public List<Storage> GetStorages()
        {
            storages.Clear();

            LoadStorages();
            return new List<Storage>(storages);
        }
        public List<Country> GetCountries()
        {
            countries.Clear();

            LoadCountries();
            return new List<Country>(countries);
        }
        #endregion


        //if (sqlDataReader["Birthday"] != DBNull.Value)
        //            {
        //                director.Birthday = (DateTime?)sqlDataReader["Birthday"];
        //            }
        //            DateTime?
    }
}
