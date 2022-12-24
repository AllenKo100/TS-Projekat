using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieHub.Models;

namespace MovieHub.Models
{
    public class MovieDBContext : DbContext
    {
        // connection string DESKTOP-G5MUTD1 miki
        public MovieDBContext(DbContextOptions<MovieDBContext> options) : base(options)
        {
        }
        public DbSet<Film> Film { get; set; }
        public DbSet<Zanr> Zanr { get; set; }
        public DbSet<FilmZanr> FilmZanr { get; set; }

        public DbSet<Watchlist> Watchlist { get; set; }

        public DbSet<Watchlist> WatchlistFilm { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Film>().ToTable("Film");
            modelBuilder.Entity<Zanr>().ToTable("Zanr");
            modelBuilder.Entity<FilmZanr>().ToTable("FilmZanr");
            modelBuilder.Entity<Watchlist>().ToTable("Watchlist");
            modelBuilder.Entity<WatchlistFilm>().ToTable("WatchlistFilm");


        }



        public DbSet<MovieHub.Models.WatchlistFilm> WatchlistFilm_1 { get; set; }
    }
}
