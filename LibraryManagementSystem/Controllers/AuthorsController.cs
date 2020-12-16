using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManagementSystem.Models
{
    public class AuthorsController : Controller
    {

        private readonly IAuthorRepository db;
        private readonly LibraryDbContext _context;
        private readonly IMemoryCache memoryCache;

        public AuthorsController(IAuthorRepository db, LibraryDbContext context, IMemoryCache memoryCache)
        {

            this.db = db;
            _context = context;
            this.memoryCache = memoryCache;
        }

        // GET: Authors
        public IActionResult Index(string searchString)
        {
            var authorr = db.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                Stopwatch stopwatch = new Stopwatch();
                List<Author> authors;

                stopwatch.Start();

                var author = from a in _context.Authors
                             select a;
                author = author.Where(s => s.Name.Contains(searchString));
                
                if (!memoryCache.TryGetValue("Authors", out authors))
                {
                    memoryCache.Set("Authors", author.ToList());
                }
                authors = memoryCache.Get("Authors") as List<Author>;
                
                stopwatch.Stop();
                ViewBag.TotalTime = stopwatch.Elapsed;
                
                return View(author.ToList());
            }
          
            return View(authorr);
        }
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Authors/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = db.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AuthorId,Name")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Create(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = db.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("AuthorId,Name")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = db.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            db.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        private bool AuthorExists(int id)
        {
            return db.Exist(id);
        }
    }
}
