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

        #endregion

        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if(commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
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
                    Strings.MainWindowViewModel_Command_ViewAllStorages,
                    new RelayCommand(param => this.ShowAllStorages())),
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
                    oneWorkspace.RequestClose += this.OnWorkspaceRequestClose;
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

        private void SetActiveWorkspace(AllMoviesViewModel workspace)
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
                oneworkspace = new AllMoviesViewModel();//////////////////////////!!!!!!!!!!!!!!!!!
                this.Workspaces.Add(oneworkspace);
            }

            this.SetActiveWorkspace(oneworkspace);
        }

        private void ShowAllStudios()
        {
            throw new NotImplementedException();
        }

        private void ShowAllActors()
        {
            throw new NotImplementedException();
        }

        private void ShowAllDirectors()
        {
            throw new NotImplementedException();
        }

        private void ShowAllGenres()
        {
            throw new NotImplementedException();
        }

        private void ShowAllStorages()
        {
            throw new NotImplementedException();
        }
        #endregion

        //public MainWindowViewModel(string customerDataFile)
        //{
        //base.DisplayName = Strings.MainWindowViewModel_DisplayName;

        //_customerRepository = new CustomerRepository(customerDataFile);
        //}


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
