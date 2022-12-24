using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Models
{
    public class Film
    {
        [ScaffoldColumn(false)]
        public int FilmID { get; set; }
        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Ocjena { get; set; }
        [StringLength(100)]
        public string Trailer { get; set; }
        [StringLength(500)]
        public string Opis { get; set;}
        [StringLength(50)]
        public string Reziser { get; set; }
        public string Poster { get; set; }
        [Display(Name = "Datum izlaska")]
        [DataType(DataType.Date)]
        public DateTime DatumIzlaska { get; set; }
        [StringLength(200)]
        public string Glumci { get; set; }
        public virtual ICollection<FilmZanr> FilmZanr { get; set; }
        public virtual ICollection<WatchlistFilm> WatchlistFilm { get; set; }

        public virtual ICollection<OmiljeniFilmovi> OmiljeniFilmovi { get; set; }

        [Display(Name = "Dodaj u listu popularnih")]
        public Boolean Popularan { get; set; }


    }
}
