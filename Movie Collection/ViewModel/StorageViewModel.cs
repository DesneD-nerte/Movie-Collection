using Movie_Collection.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    public class StorageViewModel : WorkspaceViewModel
    {
        public Storage Storage { get; set; }
        public List<Movie> Movies { get; private set; }
        public StorageViewModel()
        {
            Storage = new Storage();
        }
        public StorageViewModel(Storage newStorage)
        {
            Storage = newStorage;
            Movies = newStorage.Movies;
        }

        public int ID
        {
            get
            {
                return Storage.ID;
            }
            set
            {
                Storage.ID = value;
            }
        }
        public string Name
        {
            get
            {
                return Storage.Name;
            }
            set
            {
                Storage.Name = value;
            }
        }
    }
}
