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
    class AllStudiosViewModel : WorkspaceViewModel
    {
        DataBaseWork dataBaseStudios;
        public ObservableCollection<StudioViewModel> Studios { get; private set; } //Все Студии которые есть

        StudioViewModel selectedStudio;

        public StudioViewModel SelectedStudio
        {
            get => selectedStudio;
            set
            {
                selectedStudio = value;
                base.OnPropertyChanged("SelectedStudio");
            }
        }

        public AllStudiosViewModel(DataBaseWork dataBase, MainWindowViewModel mainWindowViewModel = null)
        {
            base.DisplayName = Strings.AllStudiosViewModel_DisplayName;

            dataBaseStudios = dataBase;
            GetAllStudios(mainWindowViewModel);
        }

        private async void GetAllStudios(MainWindowViewModel mainWindowViewModel = null)
        {
            Studios = new ObservableCollection<StudioViewModel>();
            foreach (var studio in await dataBaseStudios.GetStudios())
            {
                var newStudio = new StudioViewModel(studio, mainWindowViewModel);

                Studios.Add(newStudio);
            }
        }

        RelayCommand deleteStudioCommand;
        RelayCommand findStudioCommand;
        public ICommand DeleteStudioCommand
        {
            get
            {
                if (deleteStudioCommand == null)
                {
                    deleteStudioCommand = new RelayCommand(param =>
                    {
                        if (SelectedStudio != null)
                        {
                            SelectedStudio.DeleteStudio(dataBaseStudios);
                            Studios.Remove(SelectedStudio);
                        }
                    });
                }
                return deleteStudioCommand;
            }
        }
        public ICommand FindStudioCommand
        {
            get
            {
                if (findStudioCommand == null)
                {
                    findStudioCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            SelectedStudio = Studios.First(x => x.Name.Contains(SearchStudio));
                        }
                        catch { }
                    });
                }
                return findStudioCommand;
            }
        }

        string searchStudio = "Поиск";
        public string SearchStudio
        {
            get => searchStudio;
            set
            {
                searchStudio = value;
            }
        }
    }
}
