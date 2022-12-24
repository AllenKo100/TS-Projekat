using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class OmiljeniFilmovi
    {
        public int OmiljeniFilmoviID { get; set; }
        public int FilmID { get; set; }
        public int UserID { get; set; }

        public virtual Film Film { get; set; }
        public virtual RegistrovaniKorisnik RegistrovaniKorisnik { get; set; }
    }
}
