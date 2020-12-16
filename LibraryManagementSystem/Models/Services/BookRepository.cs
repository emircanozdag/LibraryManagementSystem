using LibraryManagementSystem.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext db;

        public BookRepository(LibraryDbContext db)
        {
            this.db = db;
        }
        void IRepositorry.IRepository<Book>.Create(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        void IRepositorry.IRepository<Book>.Delete(int id)
        {
            var book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
        }

        bool IRepositorry.IRepository<Book>.Exist(int id)
        {
            return db.Books.Any(e => e.BookId == id);
        }

        Book IBookRepository.FindWithAuthor(int? id)
        {
            return db.Books.Include(b => b.Author).FirstOrDefault(m => m.BookId == id);
        }

        IEnumerable<Book> IRepositorry.IRepository<Book>.GetAll()
        {
            
            return db.Books;
        }

        IEnumerable<Book> IBookRepository.GetAllWithAuthor()
        {
            var libraryDbContext = db.Books.Include(book => book.Author);
            return libraryDbContext.ToList();
        }

        Book IRepositorry.IRepository<Book>.GetById(int? id)
        {
            return db.Books.Find(id);
        }

        void IRepositorry.IRepository<Book>.Update(Book entity)
        {
            var entry = db.Entry(entity);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
