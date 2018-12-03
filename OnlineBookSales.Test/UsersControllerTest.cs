using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnlineBookSales.API;
using OnlineBookSales.Core.Entities;
using OnlineBookSales.Core.Interfaces;
using OnlineBookSales.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Smart.Test
{
    public class UsersControllerTest
    {
        private Mock<IRepository<Users>> _usersRepo;
        private Mock<IConfiguration> _config;
        private UsersController controller;

        public UsersControllerTest()
        {
            _usersRepo = new Mock<IRepository<Users>>();
            _config = new Mock<IConfiguration>();

            controller = new UsersController(_usersRepo.Object,  _config.Object);
        }

        [Fact]
        public void UserLogin_ReturnsSuccess_CorrectCredentials()
        {
            // Arrange
            var username = "makiep@gmail.com";
            var password = "password";
            _usersRepo.Setup(repo => repo.GetAll()).Returns(new[]
                  {
                    new Users { Id = 1, Email = username, Password = password
                  }}.AsQueryable());

 
            _config.SetupGet(m => m["Tokens:Key"]).Returns("0123456789ABCDEF");
            _config.SetupGet(m => m["Tokens:Issuer"]).Returns("http://mycodecamp.io");

            // Act
            var result = controller.Login(new LoginModel()
            {
                Email = username,
                Password = password
            }
            ) as IActionResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UserLogin_ReturnsFailure_IncorrectCrentials()
        {
            // Arrange
            var username = "user@gmail.com";
            var password = "password";
            _usersRepo.Setup(repo => repo.GetAll()).Returns(new[]
                  {
                    new Users { Id = 1, Email = "user@gmail.com", Password = "wrong"
                  }}.AsQueryable());


            _config.SetupGet(m => m["Tokens:Key"]).Returns("0123456789ABCDEF");
            _config.SetupGet(m => m["Tokens:Issuer"]).Returns("http://mycodecamp.io");

            // Act
            var result = controller.Login(new LoginModel()
            {
                Email = username,
                Password = password
            }
           ) as IActionResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Signup_ReturnsSuccess_UserSignedUp()
        {
            // Arrange
            SignUpModel model = new SignUpModel();
            model.Email = "new@gmail.com";
            model.Password = "password";
            model.ConfirmPassword = "password";
            model.FirstName = "FirstName";
            model.LastName = "LastName";

            _usersRepo.Setup(repo => repo.GetAll()).Returns(new[]
                  {
                    new Users { Id = 1, Email = "diffuser@gmail.com", Password = "diffpassword"
                  }}.AsQueryable());

            var user = new Users()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            };

            _usersRepo.Setup(repo => repo.Insert(user));
                      

            // Act
            var result = controller.SignUp(model) as IActionResult;

            // Assert
            Assert.IsType<OkResult>(result);
        }

        //[Fact]
        //public void Signup_ReturnsFailure_UserExists()
        //{
        //    // Arrange
        //    SignUpModel model = new SignUpModel();
        //    model.Email = "new@gmail.com";
        //    model.Password = "password";
        //    model.ConfirmPassword = "password";
        //    model.FirstName = "FirstName";
        //    model.LastName = "LastName";

        //    _usersRepo.Setup(repo => repo.GetAll()).Returns(new[]
        //          {
        //            new Users { Id = 1, Email = "new@gmail.com", Password = "password"
        //          }}.AsQueryable());

        //    // Act
        //    var result = controller.SignUp(model) as IActionResult;

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}
    }
}