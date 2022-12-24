using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class WatchlistFilm
    {
        public int WatchlistFilmID { get; set; }
        public int WatchlistID { get; set; }
        public int FilmId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Watchlist Watchlist { get; set; }
    }
}
