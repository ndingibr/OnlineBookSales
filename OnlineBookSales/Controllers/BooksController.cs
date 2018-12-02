using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineBookSales.Core.Entities;
using OnlineBookSales.Infrastructure;

namespace OnlineBookSales.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IRepository<Books> _booksRepo;
        private IRepository<Users> _usersRepo;
        private IRepository<Subscriptions> _subscriptionsRepo;
        private IConfiguration _config;

        public BooksController(IRepository<Books> booksRepo,
            IRepository<Users> usersRepo,
            IRepository<Subscriptions> subscriptionsRepo,
            IConfiguration config)
        {
            _booksRepo = booksRepo;
            _usersRepo = usersRepo;
            _subscriptionsRepo = subscriptionsRepo;
            _config = config;
        }

        [HttpGet("")]
        public IActionResult Books()
        {
            var books = _booksRepo.GetAll();
            return Ok(books);
        }

        [HttpGet("BooksSubscriptionByEmail")]
        public IActionResult BooksSubscriptionByEmail(string email)
        {
            var userId = _usersRepo.GetAll().FirstOrDefault(x => x.Email == email).Id;

            var booksSubscribed = (from sub in _subscriptionsRepo.GetAll()
                                   join books in _booksRepo.GetAll() on
                                   sub.BookId equals books.Id
                                   where sub.UserId == userId
                                   select new
                                   {
                                       books.Id,
                                       books.Name,
                                       books.Text,
                                       books.PurchasePrice
                                   }).ToList();

            return Ok(booksSubscribed);
        }

        [HttpGet("BooksNotSubscriptionByEmail")]
        public IActionResult BooksNotSubscriptionByEmail(string email)
        {
            var userId = _usersRepo.GetAll().FirstOrDefault(x => x.Email == email).Id;

            var booksIdSubscribed = (from sub in _subscriptionsRepo.GetAll() where sub.UserId == userId
                                  select new { bookIdName = sub.BookId }).ToList();

            var booksIdSubscribedInt = new List<int>();

            foreach (var item in booksIdSubscribed)
            {
                booksIdSubscribedInt.Add((item.bookIdName));
            }

            var booksSubscribed = (from books in _booksRepo.GetAll()
                                   .Where(x => !booksIdSubscribedInt.Contains(x.Id))
                                   select new { books }).ToList();

            return Ok(booksSubscribed);
        }

        [HttpGet("book")]
        public IActionResult Getbook(int id)
        {
            var book = _booksRepo.GetAll().FirstOrDefault(x => x.Id == id);
            return Ok(book);
        }


        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody]Subscriptions subscription)
        {
            var book = _booksRepo.GetAll().FirstOrDefault(x => x.Id == subscription.BookId);

            if (book == null)
            {
                ModelState.AddModelError("", "Book does not exists!");
                return BadRequest();
            }

            var user = _usersRepo.GetAll().FirstOrDefault(x => x.Id == subscription.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists!");
                return BadRequest();
            }

            _subscriptionsRepo.Insert(subscription);

            return Ok();
        }

        [HttpPost("unsubscribe")]
        public IActionResult UnSubscribe([FromBody]Subscriptions subscription)
        {
            var book = _booksRepo.GetAll().FirstOrDefault(x => x.Id == subscription.BookId);

            if (book == null)
            {
                ModelState.AddModelError("", "Book does not exists!");
                return BadRequest();
            }

            var user = _usersRepo.GetAll().FirstOrDefault(x => x.Id == subscription.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists!");
                return BadRequest();
            }

            var subscriptionToDelete = _subscriptionsRepo.GetAll().FirstOrDefault(x => x.BookId == subscription.BookId &&
                                            x.UserId == subscription.UserId);

            _subscriptionsRepo.Delete(subscriptionToDelete);

            return Ok();
        }

        [HttpPost("AddBook")]
        public IActionResult AddBook(Books book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                    .Select(modelError => modelError.ErrorMessage).ToList());

            _booksRepo.Insert(book);

            return Ok(book);
        }
    }
}
