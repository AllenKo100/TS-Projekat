using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Models;

namespace MovieHub.Controllers
{
    public class WatchlistFilmsController : Controller
    {
        private readonly MovieDBContext _context;

        public WatchlistFilmsController(MovieDBContext context)
        {
            _context = context;
        }

        // GET: WatchlistFilms
        public async Task<IActionResult> Index()
        {
            var movieDBContext = _context.WatchlistFilm_1.Include(w => w.Film).Include(w => w.Watchlist);
            return View(await movieDBContext.ToListAsync());
        }

        // GET: WatchlistFilms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchlistFilm = await _context.WatchlistFilm_1
                .Include(w => w.Film)
                .Include(w => w.Watchlist)
                .FirstOrDefaultAsync(m => m.WatchlistFilmID == id);
            if (watchlistFilm == null)
            {
                return NotFound();
            }

            return View(watchlistFilm);
        }

        // GET: WatchlistFilms/Create
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Film, "FilmID", "Naziv");
            ViewData["WatchlistID"] = new SelectList(_context.Watchlist, "WatchlistID", "Naziv");
            return View();
        }

        // POST: WatchlistFilms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WatchlistFilmID,WatchlistID,FilmId")] WatchlistFilm watchlistFilm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(watchlistFilm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "FilmID", "Naziv", watchlistFilm.FilmId);
            ViewData["WatchlistID"] = new SelectList(_context.Watchlist, "WatchlistID", "Naziv", watchlistFilm.WatchlistID);
            return View(watchlistFilm);
        }

        // GET: WatchlistFilms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchlistFilm = await _context.WatchlistFilm_1.FindAsync(id);
            if (watchlistFilm == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "FilmID", "Naziv", watchlistFilm.FilmId);
            ViewData["WatchlistID"] = new SelectList(_context.Watchlist, "WatchlistID", "Naziv", watchlistFilm.WatchlistID);
            return View(watchlistFilm);
        }

        // POST: WatchlistFilms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WatchlistFilmID,WatchlistID,FilmId")] WatchlistFilm watchlistFilm)
        {
            if (id != watchlistFilm.WatchlistFilmID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(watchlistFilm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchlistFilmExists(watchlistFilm.WatchlistFilmID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "FilmID", "Naziv", watchlistFilm.FilmId);
            ViewData["WatchlistID"] = new SelectList(_context.Watchlist, "WatchlistID", "Naziv", watchlistFilm.WatchlistID);
            return View(watchlistFilm);
        }

        // GET: WatchlistFilms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchlistFilm = await _context.WatchlistFilm_1
                .Include(w => w.Film)
                .Include(w => w.Watchlist)
                .FirstOrDefaultAsync(m => m.WatchlistFilmID == id);
            if (watchlistFilm == null)
            {
                return NotFound();
            }

            return View(watchlistFilm);
        }

        // POST: WatchlistFilms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var watchlistFilm = await _context.WatchlistFilm_1.FindAsync(id);
            _context.WatchlistFilm_1.Remove(watchlistFilm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WatchlistFilmExists(int id)
        {
            return _context.WatchlistFilm_1.Any(e => e.WatchlistFilmID == id);
        }
    }
}
