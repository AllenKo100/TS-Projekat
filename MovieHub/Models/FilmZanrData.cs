using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class FilmZanrData
    {
        public int ZanrID { get; set; }
        public string Naziv { get; set; }
        public bool Dodijeljen { get; set; }
    }
}
