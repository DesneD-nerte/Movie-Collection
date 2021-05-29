using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class AllGenresViewModel : WorkspaceViewModel
    {
        DataBaseWork dataBaseGenres;
        public ObservableCollection<GenreViewModel> Genres { get; private set; } //Все актеры которые есть

        GenreViewModel selectedGenre;
        public GenreViewModel SelectedGenre
        {
            get => selectedGenre;
            set
            {
                selectedGenre = value;
                base.OnPropertyChanged("SelectedGenre");
            }
        }
        public AllGenresViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllGenresViewModel_DisplayName;

            dataBaseGenres = dataBase;
            GetAllGenres();
        }

        private async void GetAllGenres()
        {
            Genres = new ObservableCollection<GenreViewModel>();
            foreach (var genre in await dataBaseGenres.GetGenres())
            {
                var newGenre = new GenreViewModel(genre);

                Genres.Add(newGenre);
            }
        }

        RelayCommand deleteCommand;
        RelayCommand findGenreCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(param =>
                    {
                        if (selectedGenre != null)
                        {
                            selectedGenre.DeleteGenre(dataBaseGenres);
                            Genres.Remove(selectedGenre);
                        }
                    });
                }
                return deleteCommand;
            }
        }
        public ICommand FindGenreCommand
        {
            get
            {
                if (findGenreCommand == null)
                {
                    findGenreCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            SelectedGenre = Genres.First(x => x.Name.Contains(SearchGenre));
                        }
                        catch { }
                    });
                }
                return findGenreCommand;
            }
        }

        string searchGenre = "Поиск";
        public string SearchGenre
        {
            get => searchGenre;
            set
            {
                searchGenre = value;
            }
        }
    }
}
