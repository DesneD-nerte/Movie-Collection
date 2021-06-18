using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using Movie_Collection.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UnitTests
{
    [TestClass]
    public class MovieTests
    {
        [TestMethod]
        public void TestIsValidPerson()
        {
            Movie movie = new Movie();
            movie.Name = "Влияние";
            movie.Description = "Описание фильма";
            movie.CountOfSeries = 1;
            movie.Duration = TimeSpan.Parse("01:59:00");
            movie.Release = DateTime.Parse("1989.01.01");
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsTrue(movie.IsValid(), "Недействительные данные");
        }

        [TestMethod]
        public void TestIsInvalidNullInfo()
        {
            Movie movie = new Movie();

            Assert.IsFalse(movie.IsValid(), "Действительные данные");
        }

        [TestMethod]
        public void TestIsInvalidEmptyName()
        {
            Movie movie = new Movie();
            movie.Name = String.Empty;
            movie.Description = "Описание фильма";
            movie.CountOfSeries = 1;
            movie.Duration = TimeSpan.Parse("01:59:00");
            movie.Release = DateTime.Parse("1989.01.01");
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsFalse(movie.IsValid(), "Название не пустое");
        }
        [TestMethod]
        public void TestIsinvalidSeriesNumber()
        {
            Movie movie = new Movie();
            movie.Name = "Влияние";
            movie.Description = "Описание фильма";
            movie.CountOfSeries = -5;
            movie.Duration = TimeSpan.Parse("01:59:00");
            movie.Release = DateTime.Parse("1989.01.01");
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsFalse(movie.IsValid(), "Должен быть недействительным");
        }
        [TestMethod]
        public void TestIsInvalidDuration()
        {
            Movie movie = new Movie();
            movie.Name = "Влияние";
            movie.Description = "Описание фильма";
            movie.CountOfSeries = 1;
            movie.Duration = TimeSpan.Parse("-01:59:00");
            movie.Release = DateTime.Parse("1989.01.01");
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsFalse(movie.IsValid(), "Корректная длительность");
        }
        [TestMethod]
        public void TestIsinvalidRelease()
        {
            Movie movie = new Movie();
            movie.Name = "Влияние";
            movie.Description = "Описание фильма";
            movie.CountOfSeries = 1;
            movie.Duration = TimeSpan.Parse("-01:59:00");
            movie.Release = DateTime.Parse("0001.01.01");
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsFalse(movie.IsValid(), "Год выхода действителен");
        }
        [TestMethod]
        public void TestIsNullProperties()
        {
            Movie movie = new Movie();
            movie.Name = "Влияние";
            movie.Description = null;
            movie.CountOfSeries = 1;
            movie.Duration = null;
            movie.Release = null;
            movie.Storage = new Storage(1, "Blu-Ray Disc");

            Assert.IsTrue(movie.IsValid(), "Данные не корректные");
        }
    }
}
