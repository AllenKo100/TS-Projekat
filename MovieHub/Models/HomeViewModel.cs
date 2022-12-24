using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class HomeViewModel
    {
        public List<Film> Popularni { get; set; }
        public List<Film> Filmovi { get; set; }
    }
}
