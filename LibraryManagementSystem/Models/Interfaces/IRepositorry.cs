using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Interfaces
{
    public class IRepositorry
    {
        public interface IRepository<T> where T : class
        {
            IEnumerable<T> GetAll();

            T GetById(int? id);

            void Create(T entity);

            void Update(T entity);

            void Delete(int id);
            bool Exist(int id);



        }
    }
}
