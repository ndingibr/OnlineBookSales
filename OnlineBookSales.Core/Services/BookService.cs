using OnlineBookSales.Core.Entities;
using OnlineBookSales.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBookSales.Core.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _booksRepo;

        public BookService(IBookRepository booksRepo)
        {
            _booksRepo = booksRepo;
        }

        public Books AddBook(Books book)
        {
            return _booksRepo.AddBook(book);
        }

        public Books GetBookById(int id)
        {
           return _booksRepo.GetBookById(id);
        }

        public IEnumerable<Books> GetAllBooks()
        {
            return _booksRepo.GetAllBooks();
        }

        public IEnumerable<Books> GetNotSubscribedBooksByUserId(int userId)
        {
            return _booksRepo.GetNotSubscribedBooksByUserId(userId);
        }

        public IEnumerable<Books> GetSubscribedBooksByUserId(int userId)
        {
            return _booksRepo.GetSubscribedBooksByUserId(userId);
        }
       
    }
}
