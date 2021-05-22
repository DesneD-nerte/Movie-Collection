using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AddStudioViewModel : WorkspaceViewModel
    {
        public ObservableCollection<ActorViewModel> AllActors { get; private set; }
        public ObservableCollection<ActorViewModel> CurrentActors { get; private set; }
        public ObservableCollection<DirectorViewModel> AllDirectors { get; private set; }
        public ObservableCollection<DirectorViewModel> CurrentlDirectors { get; private set; }
        public ObservableCollection<StudioViewModel> AllStudios { get; private set; }
        public ObservableCollection<StudioViewModel> CurrentStudios { get; private set; }
        public ObservableCollection<GenreViewModel> AllGenres { get; private set; }
        public ObservableCollection<GenreViewModel> CurrentGenres { get; private set; }
        public ObservableCollection<StorageViewModel> AllStorages { get; private set; }
        public StorageViewModel CurrentStorage { get; private set; }

        public AddStudioViewModel()
        {
            base.DisplayName = Strings.AddStudioViewModel_DisplayName;
        }
    }
}
