using Movie_Collection.DataAccess;
using Movie_Collection.Model;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Movie_Collection.ViewModel
{
    class AllDirectorsViewModel : WorkspaceViewModel
    {
        DataBaseWork dataBaseDirectors;

        public ObservableCollection<DirectorViewModel> Directors { get; private set; } //Все режиссеры которые есть
        
        DirectorViewModel selectedDirector;
        public DirectorViewModel SelectedDirector
        {
            get => selectedDirector;
            set
            {
                selectedDirector = value;
                base.OnPropertyChanged("SelectedDirector");
            }
        }
        public AllDirectorsViewModel(DataBaseWork dataBase, MainWindowViewModel mainWindowViewModel = null)
        {
            base.DisplayName = Strings.AllDirectorsViewModel_DisplayName;

            dataBaseDirectors = dataBase;
            GetAllDirectors(mainWindowViewModel);
        }

        private void GetAllDirectors(MainWindowViewModel mainWindowViewModel = null)
        {
            Directors = new ObservableCollection<DirectorViewModel>();
            foreach (var director in dataBaseDirectors.GetDirectors())
            {
                var newDirector = new DirectorViewModel(director, mainWindowViewModel);

                Directors.Add(newDirector);
            }
        }

        RelayCommand deleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(param =>
                    {
                        if (selectedDirector != null)
                        {
                            selectedDirector.DeleteDirector(dataBaseDirectors);
                            Directors.Remove(selectedDirector);
                        }
                    });
                }
                return deleteCommand;
            }
        }
    }
}
