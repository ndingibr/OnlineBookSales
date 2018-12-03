using OnlineBookSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBookSales.Core.Interfaces
{
    public interface IBookService
    {
        Books AddBook(Books book);

        Books GetBookById(int id);

        IEnumerable<Books> GetAllBooks();

        IEnumerable<Books> GetSubscribedBooksByUserId(int userId);

        IEnumerable<Books> GetNotSubscribedBooksByUserId(int userId);
    }
}
