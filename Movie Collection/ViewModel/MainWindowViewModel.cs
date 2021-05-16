using Movie_Collection.DataAccess;
using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Movie_Collection.ViewModel
{
    class MainWindowViewModel : WorkspaceViewModel
    {

        #region Fields

        ReadOnlyCollection<CommandViewModel> commands;
        ObservableCollection<WorkspaceViewModel> workspaces;
        DataBaseWork dataBaseWork;

        #endregion

        public MainWindowViewModel(string databaseConnectionString)
        {
            //base.DisplayName = Strings.MainWindowViewModel_DisplayName;

            dataBaseWork = new DataBaseWork(databaseConnectionString);
        }

        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if(commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();//Получаем команды при старте
                    commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_ViewAllMovies,
                    new RelayCommand(param => this.ShowAllMovies())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_ViewAllStudios,
                    new RelayCommand(param => this.ShowAllStudios())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_ViewAllActors,
                    new RelayCommand(param => this.ShowAllActors())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_ViewAllDirectors,
                    new RelayCommand(param => this.ShowAllDirectors())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_ViewAllGenres,
                    new RelayCommand(param => this.ShowAllGenres())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_AddMovie,
                    new RelayCommand(param => this.ShowAddMovie())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_AddStudio,
                    new RelayCommand(param => this.ShowAddStudio())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_AddActor,
                    new RelayCommand(param => this.ShowAddActor())),

                new CommandViewModel(
                    Strings.MainWindowViewModel_Command_AddDirector,
                    new RelayCommand(param => this.ShowAddDirector())),
            };
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if(workspaces == null)
                {
                    workspaces = new ObservableCollection<WorkspaceViewModel>();
                    workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel oneWorkspace in e.NewItems)
                {
                    oneWorkspace.RequestClose += this.OnWorkspaceRequestClose;//По событию закрытия workspace добавляется метод
                }
            }
            if(e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel oneWorkspace in e.OldItems)
                {
                    oneWorkspace.RequestClose -= this.OnWorkspaceRequestClose;
                }
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel oneWorkspace = sender as WorkspaceViewModel;
            oneWorkspace.Dispose();
            this.Workspaces.Remove(oneWorkspace);
        }

        #region Private Helpers

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));//Выведет на экран если уже есть

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
        }

        private void ShowAllMovies()
        {
            AllMoviesViewModel oneworkspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllMoviesViewModel)//является данным типом
                as AllMoviesViewModel;//Для явного приведения
            if(oneworkspace == null)
            {
                oneworkspace = new AllMoviesViewModel(dataBaseWork);//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAllStudios()
        {
            AllStudiosViewModel oneworkspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllStudiosViewModel)//является данным типом
                as AllStudiosViewModel;//Для явного приведения
            if (oneworkspace == null)
            {
                oneworkspace = new AllStudiosViewModel();//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAllActors()
        {
            AllActorsViewModel oneworkspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllActorsViewModel)//является данным типом
                as AllActorsViewModel;//Для явного приведения
            if (oneworkspace == null)
            {
                oneworkspace = new AllActorsViewModel();//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAllDirectors()
        {
            AllDirectorsViewModel oneworkspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllDirectorsViewModel)//является данным типом
                as AllDirectorsViewModel;//Для явного приведения
            if (oneworkspace == null)
            {
                oneworkspace = new AllDirectorsViewModel();//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAllGenres()
        {
            AllGenresViewModel oneworkspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllGenresViewModel)//является данным типом
                as AllGenresViewModel;//Для явного приведения
            if (oneworkspace == null)
            {
                oneworkspace = new AllGenresViewModel();//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAddMovie()
        {
            AddMovieViewModel oneworkspace = new AddMovieViewModel();//////////////////////////!!!!!!!!!!!!!!!!!

            this.Workspaces.Add(oneworkspace);

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAddStudio()
        {
            AddStudioViewModel oneworkspace = new AddStudioViewModel();//////////////////////////!!!!!!!!!!!!!!!!!

            this.Workspaces.Add(oneworkspace);

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAddActor()
        {
            AddActorViewModel oneworkspace = new AddActorViewModel();//////////////////////////!!!!!!!!!!!!!!!!!

            this.Workspaces.Add(oneworkspace);

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAddDirector()
        {
            AddDirectorViewModel oneworkspace = new AddDirectorViewModel();//////////////////////////!!!!!!!!!!!!!!!!!

            this.Workspaces.Add(oneworkspace);

            this.SetActiveWorkspace(oneworkspace);
        }
        #endregion


        //public string Title { get; set; } = "Коллекция фильмов";

        private string title = "Коллекция фильмов";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (Equals(value, Title))//Для избегания бесконечных переназначений
                    return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

    }
}
