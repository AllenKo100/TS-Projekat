using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Models;
using System.Security.Claims;

namespace MovieHub.Controllers
{

    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WatchlistController : Controller
    {
        private readonly MovieDBContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<RegistrovaniKorisnik> _userManager;

        public WatchlistController(MovieDBContext context, Microsoft.AspNetCore.Identity.UserManager<RegistrovaniKorisnik> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Watchlist
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var watchlist = await _context.Watchlist.ToListAsync();
            var watchlist = from w in _context.Watchlist select w;
            watchlist = watchlist.Where(w => w.UserID == userId);
            
            return View(await watchlist.AsNoTracking().ToListAsync());
        }

        // GET: Watchlist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* var watchlist = await _context.Watchlist
                 .FirstOrDefaultAsync(m => m.WatchlistID == id); */

            Watchlist watchlist = await _context.Watchlist
        .Include(w => w.Filmovi)
        .ThenInclude(f => f.Film)
        .ThenInclude(f => f.FilmZanr)
        .ThenInclude(f => f.Zanr)
        .AsNoTracking()
        .FirstOrDefaultAsync(w => w.WatchlistID == id);


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (watchlist == null || watchlist.UserID != userId)
            {
                return NotFound();
            }

            return View(watchlist);
        }

        // GET: Watchlist/Create
        public IActionResult Create()
        {
            ViewData["Filmovi"] = new MultiSelectList(_context.Film, "FilmID", "Naziv");
            return View();
        }

        // POST: Watchlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WatchlistID,Naziv")] Watchlist watchlist, int[] FilmID)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                watchlist.UserID = userId;
                var filmovi = _context.Film.Where(f => FilmID.Contains(f.FilmID)).ToList();
                if (watchlist.Filmovi == null) watchlist.Filmovi = new List<WatchlistFilm>();
                foreach (var film in filmovi)
                {
                    WatchlistFilm wf = new WatchlistFilm();
                    wf.Watchlist = watchlist;
                    wf.Film = film;
                    watchlist.Filmovi.Add(wf);
                }
                _context.Add(watchlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(watchlist);
        }

        // GET: Watchlist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var watchlist = await _context.Watchlist
                  .Include(w => w.Filmovi)
                  .ThenInclude(w => w.Film)
                  .FirstOrDefaultAsync(w => w.WatchlistID == id);
            var selektovani = dajSelektovaneId(watchlist);
            ViewBag.Filmovi = new MultiSelectList(_context.Film, "FilmID", "Naziv", selektovani);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (watchlist == null || watchlist.UserID != userId)
            {
                return NotFound();
            }
            return View(watchlist);
        }

        

        private List<int> dajSelektovaneId(Watchlist watchlist)
        {
            List<int> selektovani = new List<int>();
            foreach (var v in watchlist.Filmovi)
            {
                selektovani.Add(v.FilmId);
            }
            return selektovani;
        }

        // POST: Watchlist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WatchlistID,Naziv,UserID")] Watchlist watchlist, int[] FilmID)
        {
            if (id != watchlist.WatchlistID)
            {
                return NotFound();
            }
            var watchlistToUpdate = await _context.Watchlist
                  .Include(w => w.Filmovi)
                  .ThenInclude(w => w.Film)
                  .FirstOrDefaultAsync(w => w.WatchlistID == id);

            if (ModelState.IsValid)
            {
                try
                {
                   
                    UpdateWatchlistFilm(FilmID, watchlistToUpdate);
                    _context.Update(watchlistToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchlistExists(watchlist.WatchlistID))
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (watchlistToUpdate.UserID != userId) return NotFound();
            var selektovani = dajSelektovaneId(watchlist);
            ViewBag.Filmovi = new MultiSelectList(_context.Film, "FilmID", "Naziv", selektovani);

            ViewBag.Filmovi = new MultiSelectList(_context.Film, "FilmID", "Naziv", selektovani);
            return View(watchlistToUpdate);
        }
        // monster by mirzoroza v2
        private void UpdateWatchlistFilm(int[] odabraniFilmovi, Watchlist watchlistToUpdate)
        {
            if (odabraniFilmovi == null)
            {
                watchlistToUpdate.Filmovi = new List<WatchlistFilm>();
                return;
            }

            var odabraniFilmoviHS = new HashSet<int>(odabraniFilmovi);
            var p = watchlistToUpdate.Filmovi.Select(f => f.FilmId);
            HashSet<int> prethodniSelekt;
            if (p != null)
                prethodniSelekt = new HashSet<int>(p);
            else
                prethodniSelekt = new HashSet<int>();
            foreach (var film in _context.Film)
            {
                if (odabraniFilmoviHS.Contains(film.FilmID))
                {
                    if (!prethodniSelekt.Contains(film.FilmID))
                    {
                        watchlistToUpdate.Filmovi.Add(new WatchlistFilm { WatchlistID = watchlistToUpdate.WatchlistID, 
                                                                            FilmId = film.FilmID });
                    }
                }
                else
                {

                    if (prethodniSelekt.Contains(film.FilmID))
                    {
                        WatchlistFilm wf = watchlistToUpdate.Filmovi.FirstOrDefault(f => f.FilmId == film.FilmID);
                        _context.Remove(wf);
                    }
                }
            }
        }

        // GET: Watchlist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchlist = await _context.Watchlist
                .FirstOrDefaultAsync(m => m.WatchlistID == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (watchlist == null || watchlist.UserID != userId)
            {
                return NotFound();
            }

            return View(watchlist);
        }

        // POST: Watchlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var watchlist = await _context.Watchlist.FindAsync(id);
            _context.Watchlist.Remove(watchlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WatchlistExists(int id)
        {
            return _context.Watchlist.Any(e => e.WatchlistID == id);
        }
    }
}
