using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class FilmZanr
    {
        public int FilmZanrID { get; set; }
        public int FilmID { get; set; }
        public int ZanrId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Zanr Zanr { get; set; }
    }
}
