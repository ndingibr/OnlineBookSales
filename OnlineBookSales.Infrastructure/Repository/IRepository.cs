using System.Collections.Generic;
using System.Linq;

namespace OnlineBookSales.Infrastructure
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