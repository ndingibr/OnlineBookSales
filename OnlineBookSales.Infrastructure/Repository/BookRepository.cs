using OnlineBookSales.Core.Entities;
using OnlineBookSales.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineBookSales.Infrastructure.Repository
{
    public class BookRepository : IBookRepository
    {
        private IRepository<Books> _booksRepo;
        private IRepository<Subscriptions> _subscriptionsRepo;

        public BookRepository(IRepository<Books> booksRepo,
            IRepository<Subscriptions> subscriptionsRepo)
        {
            _booksRepo = booksRepo;
            _subscriptionsRepo = subscriptionsRepo;
        }

        public IEnumerable<Books> GetAllBooks()
        {
            var books = _booksRepo.GetAll();
            return books;
        }

        public Books GetBookById(int id)
        {
            var book = _booksRepo.GetAll().FirstOrDefault(x => x.Id == id);
            return book;
        }

        public Books AddBook(Books book)
        {
            _booksRepo.Insert(book);

            return book;
        }

        public IEnumerable<Books> GetNotSubscribedBooksByUserId(int userId)
        {
            var booksIdSubscribed = (from sub in _subscriptionsRepo.GetAll()
                                     where sub.UserId == userId
                                     select new { bookIdName = sub.BookId }).ToList();

            var booksIdSubscribedInt = new List<int>();

            foreach (var item in booksIdSubscribed)
            {
                booksIdSubscribedInt.Add((item.bookIdName));
            }

            var booksSubscribed = (from books in _booksRepo.GetAll()
                                   .Where(x => !booksIdSubscribedInt.Contains(x.Id))
                                   select new Books
                                   {
                                       Id = books.Id,
                                       Name = books.Name,
                                       PurchasePrice = books.PurchasePrice,
                                       Text = books.Text
                                   }).ToList();


            return booksSubscribed;
        }

        public IEnumerable<Books> GetSubscribedBooksByUserId(int userId)
        {
            var booksSubscribed = (from sub in _subscriptionsRepo.GetAll()
                                   join books in _booksRepo.GetAll() on
                                   sub.BookId equals books.Id
                                   where sub.UserId == userId
                                   select new Books
                                   {
                                       Id = books.Id,
                                       Name = books.Name,
                                       PurchasePrice = books.PurchasePrice,
                                       Text = books.Text
                                   }).ToList();

            return booksSubscribed;

        }
    }
}
