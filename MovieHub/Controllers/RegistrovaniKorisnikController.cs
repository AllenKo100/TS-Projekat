using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovieHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHub.Controllers
{
    public class RegistrovaniKorisnikController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<RegistrovaniKorisnik> _userManager;

        public RegistrovaniKorisnikController(UserManager<RegistrovaniKorisnik> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: RegistrovaniKorisnikController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var allUsers = _userManager.Users.ToList();
            return View(allUsers);
        }

        // GET: RegistrovaniKorisnikController/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allUsers = _userManager.Users.ToList();
            var korisnik = allUsers.Find(k => k.Id == id.ToString());

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: RegistrovaniKorisnikController/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegistrovaniKorisnikController/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegistrovaniKorisnikController/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allUsers = _userManager.Users.ToList();
            var korisnik = allUsers.Find(k => k.Id == id.ToString());

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: RegistrovaniKorisnikController/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegistrovaniKorisnikController/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allUsers = _userManager.Users.ToList();
            var korisnik = allUsers.Find(k => k.Id == id.ToString());

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: RegistrovaniKorisnikController/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection("Server=DESKTOP-AAU2QSM\\SQLEXPRESS;Database=moviedb;Trusted_Connection=True;"))
                {
                    sqlCon.Open();

                    string query = "DELETE FROM AspNetUsers WHERE Id = @Id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Id", id);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
