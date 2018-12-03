using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineBookSales.Core.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void AddRange(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
    }
}
