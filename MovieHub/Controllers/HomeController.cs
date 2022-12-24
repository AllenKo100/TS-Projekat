using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieHub.Models;

namespace MovieHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDBContext _context;

        public HomeController(ILogger<HomeController> logger, MovieDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Popularni = await _context.Film.Where(film => film.Popularan == true).Include(f => f.FilmZanr).ThenInclude(f => f.Zanr).ToListAsync();
            model.Filmovi = await _context.Film.Where(film => film.FilmID >= 50 && film.FilmID < 60).Include(f => f.FilmZanr).ThenInclude(f => f.Zanr).ToListAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
