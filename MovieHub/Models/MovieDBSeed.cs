using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace MovieHub.Models
{
    public class MovieDBSeed
    {
        private static string posterBaseUrl = "https://image.tmdb.org/t/p/w342";
        private static string trailerBaseUrl = "https://www.youtube.com/watch?v=";
        public static void Napuni(MovieDBContext context)
        {
            
            context.Database.EnsureCreated();
            var praznaBaza = context.Film;
            if (praznaBaza.Any()) return;
            TmdbAPI tmdbAPI = new TmdbAPI();
            List<Zanr> zanrovi = tmdbAPI.dajZanrove();
            context.AddRangeAsync(zanrovi).Wait();
            context.SaveChanges();
            SearchContainer<SearchMovie> movies = tmdbAPI.dajFilmove();

            foreach (SearchMovie m in movies.Results)

            {
                Movie movie = tmdbAPI.dajFilm(m.Id);
                Film film = new Film();
                film.Naziv = movie.Title;
                film.DatumIzlaska = (DateTime) movie.ReleaseDate;
                film.Ocjena = (decimal) movie.VoteAverage;
                film.Poster =  posterBaseUrl + movie.PosterPath;
                if (movie.Overview.Length > 500)
                    film.Opis = movie.Overview.Substring(0, 500);
                else
                    film.Opis = movie.Overview;
                int brojac = 1;
                String glumci = "";
                foreach(Cast cast in movie.Credits.Cast)
                {
                    if (brojac == 5) break;
                    glumci = cast.Name + ",";
                    brojac++;
                }
                film.Glumci = glumci;
                foreach (Genre g in movie.Genres)
                {
                    var zanr = context.Zanr.FirstOrDefault(z => z.Naziv.Equals(g.Name));
                    if (zanr != null)
                    {
                        FilmZanr filmZanr = new FilmZanr() { FilmID = film.FilmID, ZanrId = zanr.ZanrID };
                        film.FilmZanr = new List<FilmZanr>();
                        film.FilmZanr.Add(filmZanr);
                    }
                }
                if(movie.Videos.Results.Count > 0)
                    film.Trailer =  trailerBaseUrl + movie.Videos.Results.ElementAt(0).Key;
                film.Popularan = true;
                context.Add(film);
            }

            for(int i = 0; i < 5; ++i)
            {
                movies = tmdbAPI.dajNajboljeFilmove(i);

                foreach (SearchMovie m in movies.Results)

                {
                    Movie movie = tmdbAPI.dajFilm(m.Id);
                    Film film = new Film();
                    film.Naziv = movie.Title;
                    film.DatumIzlaska = (DateTime)movie.ReleaseDate;
                    film.Ocjena = (decimal)movie.VoteAverage;
                    film.Poster = posterBaseUrl + movie.PosterPath;
                    if (movie.Overview.Length > 500)
                        film.Opis = movie.Overview.Substring(0, 500);
                    else
                        film.Opis = movie.Overview;
                    int brojac = 1;
                    String glumci = "";
                    foreach (Cast cast in movie.Credits.Cast)
                    {
                        if (brojac == 5) break;
                        glumci = cast.Name + ",";
                        brojac++;
                    }
                    film.Glumci = glumci;
                    foreach (Genre g in movie.Genres)
                    {
                        var zanr = context.Zanr.FirstOrDefault(z => z.Naziv.Equals(g.Name));
                        if (zanr != null)
                        {
                            FilmZanr filmZanr = new FilmZanr() { FilmID = film.FilmID, ZanrId = zanr.ZanrID };
                            film.FilmZanr = new List<FilmZanr>();
                            film.FilmZanr.Add(filmZanr);
                        }
                    }
                    if (movie.Videos.Results.Count > 0)
                    {
                        film.Trailer = trailerBaseUrl + movie.Videos.Results.ElementAt(0).Key;
                    }
                    context.Add(film);
                }

            }
            context.SaveChanges();

        }
    }

    
}
