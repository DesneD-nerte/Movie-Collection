﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Collection.Model
{
    class Storage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}