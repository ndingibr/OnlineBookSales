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
using OnlineBookSales.Core.Interfaces;

namespace OnlineBookSales.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBookService _booksService;
        private IRepository<Users> _usersRepo;
        private ISubscriptionsRepository _subscriptionsRepo;
        private IConfiguration _config;

        public BooksController(IBookService booksService,
            IRepository<Users> usersRepo,
            ISubscriptionsRepository subscriptionsRepo,
            IConfiguration config)
        {
            _booksService = booksService;
            _usersRepo = usersRepo;
            _subscriptionsRepo = subscriptionsRepo;
            _config = config;
        }

        [HttpGet("")]
        public IActionResult Books()
        {
            var books = _booksService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("BooksSubscriptionByEmail")]
        public IActionResult BooksSubscriptionByEmail(string email)
        {
            var userId = _usersRepo.GetAll().FirstOrDefault(x => x.Email == email).Id;
            var unSubscribedBooks = _booksService.GetSubscribedBooksByUserId(userId);
            return Ok(unSubscribedBooks);
        }

        [HttpGet("BooksNotSubscriptionByEmail")]
        public IActionResult BooksNotSubscriptionByEmail(string email)
        {
            var userId = _usersRepo.GetAll().FirstOrDefault(x => x.Email == email).Id;
            var unSubscribedBooks = _booksService.GetNotSubscribedBooksByUserId(userId);
            return Ok(unSubscribedBooks);
        }

        [HttpGet("book")]
        public IActionResult Getbook(int id)
        {
            var book = _booksService.GetBookById(id);
            return Ok(book);
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody]Subscriptions subscription)
        {
            var user = _usersRepo.GetAll().FirstOrDefault(x => x.Id == subscription.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists!");
                return BadRequest();
            }

            var subscribedToBook = _booksService.GetSubscribedBooksByUserId(subscription.UserId).Where(x => x.Id == subscription.BookId).FirstOrDefault();

            if (subscribedToBook != null)
            {
                ModelState.AddModelError("", "You are already subscribed to book " + subscribedToBook.Name);
                return BadRequest();
            }

            var bookExist = _booksService.GetAllBooks().FirstOrDefault(x => x.Id == subscription.BookId);

            if (bookExist == null)
            {
                ModelState.AddModelError("", "Book does not exists!");
                return BadRequest();
            }

            subscription.Id = 0;

            _subscriptionsRepo.Subscribe(subscription);

            return Ok();
        }

        [HttpPost("unsubscribe")]
        public IActionResult UnSubscribe([FromBody]Subscriptions subscription)
        {
            var book = _booksService.GetAllBooks().FirstOrDefault(x => x.Id == subscription.BookId);

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

            var subscribedToBook = _booksService.GetNotSubscribedBooksByUserId(subscription.UserId).Where(x => x.Id == subscription.BookId).FirstOrDefault();

            if (subscribedToBook != null)
            {
                ModelState.AddModelError("", "You are not subscribed to book " + subscribedToBook.Name);
                return BadRequest();
            }

            _subscriptionsRepo.UnSubscribe(subscription);

            return Ok();
        }

        [HttpPost("AddBook")]
        public IActionResult AddBook(Books book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                    .Select(modelError => modelError.ErrorMessage).ToList());

           var newBook = _booksService.AddBook(book);

            return Ok(newBook);
        }
    }
}
