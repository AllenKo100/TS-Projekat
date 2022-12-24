using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities;

namespace MovieHub.Models
{
    public class TmdbAPI
    {
       // private string apiKey = "14ebcb8fed5d27053be78c9e824bdb3f";
        private TMDbClient client = new TMDbClient("8a4544b355722aa0fb2146c8e7f774b2");

        public SearchContainer<SearchMovie> dajFilmove()
        {
            SearchContainer<SearchMovie> movies = client.GetMoviePopularListAsync().Result;
            return movies;
        }

        public SearchContainer<SearchMovie> dajNajboljeFilmove(int page)
        {
            SearchContainer<SearchMovie> movies = client.GetMovieTopRatedListAsync(null, page, null).Result;
            return movies;
        }

        public List<Zanr> dajZanrove()
        {
            List<Genre> genres = client.GetMovieGenresAsync().Result;
            List<Zanr> zanrovi = new List<Zanr>();
            foreach(Genre g in genres)
            {
                zanrovi.Add(new Zanr() { Naziv = g.Name });
            }
            return zanrovi;
        }

        public Movie dajFilm(int id)
        {
            
             return client.GetMovieAsync(id, MovieMethods.Credits | MovieMethods.Videos).Result;
        }

        public void dajTrejlerZaFilm(int id)
        {
            
        }


    }
}
