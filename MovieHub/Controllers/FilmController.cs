using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Models;

namespace MovieHub.Controllers
{
    public class FilmController : Controller
    {
        private readonly MovieDBContext _context;

        public FilmController(MovieDBContext context)
        {
            _context = context;
        }

        // GET: Film
        public async Task<IActionResult> Index(string sortOrder, string currentSearch, string searchString, int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentSearch"] = searchString;
           
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentSearch;
            }
            ViewData["CurrentFilter"] = searchString;
            var movies = from s in _context.Film
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                 movies = movies.Where(s => s.Naziv.Contains(searchString));
   
            }
            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(m => m.Naziv);
                    break;
                case "Date":
                    movies = movies.OrderBy(m => m.DatumIzlaska);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(m => m.DatumIzlaska);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Naziv);
                    break;
            }
            int pageSize = 20;
            return View(await PaginatedList<Film>.CreateAsync(movies.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Film/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film.Include(f => f.FilmZanr).ThenInclude(f => f.Zanr)
                .FirstOrDefaultAsync(m => m.FilmID == id);
            ViewBag.popularni = new List<Film>(); 
            List<Film> list = (await _context.Film.Where(f => f.Popularan == true).Include(f => f.FilmZanr).ThenInclude(f => f.Zanr).ToListAsync());
            list.RemoveRange(6, list.Count - 6);
            ViewBag.popularni = list;

            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Film/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.Zanrovi =  new MultiSelectList(_context.Zanr, "ZanrID", "Naziv");
            return View();
        }

        // POST: Film/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Ocjena,Trailer,Opis,Reziser,Poster,DatumIzlaska,Glumci,Popularan")] Film film, int[] ZanrID)
        {
            if (ModelState.IsValid)
            {
                var zanrovi = _context.Zanr.Where(z => ZanrID.Contains(z.ZanrID)).ToList();
                if (film.FilmZanr == null) film.FilmZanr = new List<FilmZanr>();
                foreach (var zanr in zanrovi)
                {
                    FilmZanr filmZanr = new FilmZanr() { FilmID = film.FilmID, ZanrId = zanr.ZanrID };
                    film.FilmZanr.Add(filmZanr);
                }
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Film/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film.Include(f => f.FilmZanr).ThenInclude(f => f.Zanr).
                AsNoTracking().FirstOrDefaultAsync(f => f.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }
            PopuniZanrove(film);
            return View(film);
        }
        private void PopuniZanrove(Film film)
        {
            var sviZanrovi = _context.Zanr;
            var filmZanr = new HashSet<int>(film.FilmZanr.Select(z => z.ZanrId));
            var viewModel = new List<FilmZanrData>();
            foreach (var zanr in sviZanrovi)
            {
                viewModel.Add(new FilmZanrData
                {
                    ZanrID = zanr.ZanrID,
                    Naziv = zanr.Naziv,
                    Dodijeljen = filmZanr.Contains(zanr.ZanrID)
                });
            }
            ViewData["Zanrovi"] = viewModel;
        }

        // POST: Film/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, string[] odabraniZanrovi)
        {
            /*  [Bind("Naziv,Ocjena,Trailer,Opis,Reziser,Poster,DatumIzlaska")] Film film
              if (id != film.FilmID)
              {
                  return NotFound();
              }

              if (ModelState.IsValid)
              {
                  try
                  {
                      _context.Update(film);
                      await _context.SaveChangesAsync();
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!FilmExists(film.FilmID))
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
              return View(film);
            */
            if (id == null)
            {
                return NotFound();
            }

            var filmToUpdate = await _context.Film
                .Include(f => f.FilmZanr)
                .ThenInclude(f => f.Zanr)
                .FirstOrDefaultAsync(f => f.FilmID == id);

            if (await TryUpdateModelAsync<Film>(
                filmToUpdate,
                "",
                f => f.Naziv, f => f.Ocjena, f => f.Trailer, f => f.Opis, f => f.Reziser, f => f.Poster, f => f.DatumIzlaska, f => f.Glumci))
            {
               
                UpdateFilmZanr(odabraniZanrovi, filmToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateFilmZanr(odabraniZanrovi, filmToUpdate);
            PopuniZanrove(filmToUpdate);
            return View(filmToUpdate);
        }

        private void UpdateFilmZanr(string[] odabraniZanrovi, Film filmToUpdate)
        {
            if (odabraniZanrovi == null)
            {
                filmToUpdate.FilmZanr = new List<FilmZanr>();
                return;
            }

            var odabraniZanroviHS = new HashSet<string>(odabraniZanrovi);
            var filmCourses = new HashSet<int>
                (filmToUpdate.FilmZanr.Select(f => f.ZanrId ));
            foreach (var zanr in _context.Zanr)
            {
                if (odabraniZanroviHS.Contains(zanr.ZanrID.ToString()))
                {
                    if (!filmCourses.Contains(zanr.ZanrID))
                    {
                        filmToUpdate.FilmZanr.Add(new FilmZanr { FilmID = filmToUpdate.FilmID, ZanrId = zanr.ZanrID });
                    }
                }
                else
                {

                    if (filmCourses.Contains(zanr.ZanrID))
                    {
                        FilmZanr courseToRemove = filmToUpdate.FilmZanr.FirstOrDefault(i => i.ZanrId == zanr.ZanrID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }

        // GET: Film/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Film.FindAsync(id);
            _context.Film.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Film.Any(e => e.FilmID == id);
        }
    }
}
