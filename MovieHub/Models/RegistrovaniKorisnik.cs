using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class RegistrovaniKorisnik : IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string AppUsername { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string SlikaProfila { get; set; }
        public DateTime RokPretplate { get; set; }

        public IList<Watchlist> Watchlists { get; set; }

        public virtual ICollection<OmiljeniFilmovi> OmiljeniFilmovi { get; set; }
    }
}
