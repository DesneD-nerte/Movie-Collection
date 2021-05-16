using Movie_Collection.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.ViewModel
{
    class AddMovieViewModel : WorkspaceViewModel
    {
        public AddMovieViewModel()
        {
            base.DisplayName = Strings.AddMovieViewModel_DisplayName;
        }
    }
}
