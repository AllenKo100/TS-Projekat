using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class WatchlistViewModel
    {
        public Watchlist Watchlist { get; set; }
        public IEnumerable<Film> FilmList { get; set; }
        public IEnumerable<int> SelectedFilms { get; set; }
    }
}
