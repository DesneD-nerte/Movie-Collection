using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Director
    {
        public int ID { get; private set; } 
        public string Name { get; private set; }    
        public string Surname { get; private set; } 
        public string Patronymic { get; private set; }
        public List<Movie> Movies { get; private set; }

    }
}
