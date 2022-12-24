using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class Zanr
    {
        [ScaffoldColumn(false)]
        public int ZanrID { get; set; }
        [StringLength(50)]
        public string Naziv { get; set; }

        public virtual ICollection<FilmZanr> FilmZanr { get; set; }

    }
}
