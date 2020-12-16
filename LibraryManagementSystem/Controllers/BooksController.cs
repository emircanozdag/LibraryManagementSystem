using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Services;
using LibraryManagementSystem.Models.Interfaces;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository db;
        private readonly LibraryDbContext _context;
        private readonly IMemoryCache memoryCache;

        public BooksController(IBookRepository db, LibraryDbContext context, IMemoryCache memoryCache)
        {
            this.db = db;
            _context = context;
            this.memoryCache = memoryCache;
        }

        // GET: Books
        public IActionResult Index(string searchString)
        {
            var bookss = db.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                List<Book> books;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var book = from a in _context.Books
                           select a;
                book = book.Where(s => s.Title.Contains(searchString));
                if (!memoryCache.TryGetValue("Books", out books))
                {
                    memoryCache.Set("Books", book.ToList());
                }
                books = memoryCache.Get("Books") as List<Book>;
                stopwatch.Stop();
                ViewBag.TotalTime = stopwatch.Elapsed;
                return View(books.ToList());
            }

            return View(bookss);
        }

        // GET: Books/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = db.FindWithAuthor(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("BookId,Title,AuthorId")] Book book)
        {
            
            if (ModelState.IsValid)
            {
                db.Create(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = db.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("BookId,Title,AuthorId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(book);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = db.FindWithAuthor(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            db.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return db.Exist(id);
        }
    }
}
