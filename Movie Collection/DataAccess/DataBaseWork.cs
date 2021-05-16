using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Movie_Collection.DataAccess
{
    class DataBaseWork
    {
        string databaseConnectionString;
        public DataBaseWork(string serverName)
        {
            databaseConnectionString = serverName;
            LoadMovies();
        }

        List<Movie> movies = new List<Movie>();
        List<Studio> studios = new List<Studio>();
        List<Actor> actors = new List<Actor>();
        List<Director> directors = new List<Director>();
        List<Genre> genres = new List<Genre>();
        List<Storage> storages = new List<Storage>();

        private void LoadMovies()
        {
            using (SqlConnection sqlConnection = new SqlConnection(databaseConnectionString))
            {
                string query = "SELECT * FROM Movie";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while(sqlDataReader.Read())
                {
                    Movie movie = new Movie(
                        Convert.ToInt32(sqlDataReader["Id_movie"]), sqlDataReader["Name"].ToString(), sqlDataReader["Description"].ToString(),
                   storages.Find(x => x.ID == Convert.ToInt32(sqlDataReader["Id_storage"])), Convert.ToInt32(sqlDataReader["Number_series"]), sqlDataReader["Duration"].ToString(), sqlDataReader["Release"].ToString());

                    movies.Add(movie);
                }

                sqlDataReader.Close();
                sqlConnection.Close();
            }
        }

        public List<Movie> GetMovies()
        {
            return new List<Movie>(movies);
        }

        private void LoadStudios()
        {

        }
        private void LoadActors()
        {

        }
        private void LoadDirectors()
        {

        }
        private void LoadGenres()
        {

        }
        private void LoadStorages()
        {

        }
    }
}
