using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Models.Interfaces.IRepositorry;

namespace LibraryManagementSystem.Models.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetAllWithAuthor();
        Book FindWithAuthor(int? id);
    }
}
