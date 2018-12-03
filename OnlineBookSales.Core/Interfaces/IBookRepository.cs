using OnlineBookSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBookSales.Core.Interfaces
{
    public interface IBookRepository //: IRepository<Books>
    {
        Books AddBook(Books book);
        Books GetBookById(int id);
        IEnumerable<Books> GetAllBooks();
        IEnumerable<Books> GetNotSubscribedBooksByUserId(int userId);
        IEnumerable<Books> GetSubscribedBooksByUserId(int userId);
    }
}
