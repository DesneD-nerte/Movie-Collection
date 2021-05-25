using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

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

        public AllStudiosViewModel(DataBaseWork dataBase)
        {
            base.DisplayName = Strings.AllStudiosViewModel_DisplayName;

            dataBaseStudios = dataBase;
            GetAllStudios();
        }

        private void GetAllStudios()
        {
            Studios = new ObservableCollection<StudioViewModel>();
            foreach (var studio in dataBaseStudios.GetStudios())
            {
                var newStudio = new StudioViewModel(studio);

                Studios.Add(newStudio);
            }
        }
    }
}
