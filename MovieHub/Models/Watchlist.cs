using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class Watchlist
    {
        public int WatchlistID { get; set; }
        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        public string UserID { get; set; }

        public RegistrovaniKorisnik RegistrovaniKorisnik { get; set; }
        
        public virtual ICollection<WatchlistFilm> Filmovi { get; set; }


    }
}
