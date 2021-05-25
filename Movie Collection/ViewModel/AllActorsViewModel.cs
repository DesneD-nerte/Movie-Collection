using Movie_Collection.DataAccess;
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
    class AllActorsViewModel : WorkspaceViewModel
    {
        DataBaseWork dataBaseActors;
        public ObservableCollection<ActorViewModel> Actors { get; private set; } //Все актеры которые есть
        ActorViewModel selectedActor;
        public ActorViewModel SelectedActor
        {
            get => selectedActor;
            set
            {
                selectedActor = value;
                base.OnPropertyChanged("SelectedActor");
            }
        }
        public AllActorsViewModel(DataBaseWork dataBase, MainWindowViewModel mainWindowViewModel = null)
        {
            base.DisplayName = Strings.AllActorsViewModel_DisplayName;

            dataBaseActors = dataBase;
            GetAllActors(mainWindowViewModel);
        }

        private void GetAllActors(MainWindowViewModel mainWindowViewModel = null)
        {
            Actors = new ObservableCollection<ActorViewModel>();
            foreach (var actor in dataBaseActors.GetActors())
            {
                var newActor = new ActorViewModel(actor, mainWindowViewModel);

                Actors.Add(newActor);
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
                        if (selectedActor != null)
                        {
                            selectedActor.DeleteActor(dataBaseActors);
                            Actors.Remove(selectedActor);
                        }
                    });
                }
                return deleteCommand;
            }
        }

    }
}
