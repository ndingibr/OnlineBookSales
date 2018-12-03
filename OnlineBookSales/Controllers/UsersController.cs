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
    public class UsersController : Controller
    {
        private IRepository<Users> _usersRepo;
        private IConfiguration _config;

        public UsersController(IRepository<Users> usersRepo, 
            IConfiguration config)
        {
            _usersRepo = usersRepo;
            _config = config;
        }

        [HttpGet("")]
        public IActionResult Users()
        {
            var users = _usersRepo.GetAll();
            return Ok(users);
        }

        [HttpGet("user")]
        public IActionResult GetUser(string email)
        {
            var user =  _usersRepo.GetAll().FirstOrDefault(x => x.Email == email);
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _usersRepo.GetAll()
                .FirstOrDefault(x => x.Email == model.Email);

            if (user.Password != model.Password)
                return BadRequest("Wrong password");

           
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
              _config["Tokens:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }


        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public IActionResult SignUp([FromBody]SignUpModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                    .Select(modelError => modelError.ErrorMessage).ToList());

            var existingUser = _usersRepo.GetAll().FirstOrDefault(x => x.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "User already exists!");
                return BadRequest();
            }

            var user = new Users()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            };

            _usersRepo.Insert(user);
      
            return Ok();
        }

        private string GenerateToken(string username, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(_config["Tokens:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, username)
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
