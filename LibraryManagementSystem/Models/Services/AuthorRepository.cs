using LibraryManagementSystem.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Models.Interfaces.IRepositorry;

namespace LibraryManagementSystem.Models.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext db;

        public AuthorRepository(LibraryDbContext db)
        {
            this.db = db;
        }

        public bool Exist(int id)
        {
            return db.Authors.Any(e => e.AuthorId == id);
        }

        public Author GetWithBooks(int id)
        {
            var book = db.Authors.Find(id);
            return book;
        }

        void IRepository<Author>.Create(Author entity)
        {
            db.Add(entity);
            db.SaveChanges();
        }

        void IRepository<Author>.Delete(int id)
        {
            var author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
        }

        IEnumerable<Author> IRepository<Author>.GetAll()
        {
            return db.Authors; 
        }

        IEnumerable<Author> IAuthorRepository.GetAllWithBooks()
        {
            return db.Authors;
        }

        Author IRepository<Author>.GetById(int? id)
        {
            return db.Authors
                .FirstOrDefault(m => m.AuthorId == id);
        }


        void IRepository<Author>.Update(Author entity)
        {
            var entry = db.Entry(entity);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
