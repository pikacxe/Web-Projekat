﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.Impl;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;


namespace Projekat.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        string _salt = ConfigurationManager.AppSettings["PasswdSalt"];
        string jwt_secret = ConfigurationManager.AppSettings["JwtSecretKey"];
        IUserRepository userRepo = new UserRepository();
        IUserDao userDao = new UserDAO();

        [HttpGet]
        [ActionName("all")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult GetAllUsers()
        {
            return Ok(DB.UsersList.Where(x => !x.isDeleted));
        }
        [HttpGet]
        [ActionName("user")]
        [AllowAnonymous]
        public IHttpActionResult GetById(int id)
        {
            User temp = userDao.FindById(id);
            if(temp == default(User))
            {
                return NotFound();
            }
            return Ok(temp);
        }

        [HttpGet]
        [ActionName("current")]
        public IHttpActionResult GetUsername()
        {
            User current = userDao.FindByUsername(User.Identity.Name);
            if(current == default(User))
            {
                return NotFound();
            }
            return Ok(new { id = current.ID, name = current.Username, role = current.Role.ToString() });
        }

        [HttpPost]
        [ActionName("login")]
        [AllowAnonymous]
        public IHttpActionResult LogIn([FromBody] LoginRequest req)
        {
            User current = userDao.FindByUsername(req.username);
            if (current == default(User))
            {
                return BadRequest("No account associated with provided username!");
            }
            if (VerifyPassword(req.password, current.Password))
            {
                return Ok(GenerateJwtToken(current));
            }
            return BadRequest("Invalid password!");
        }

        [HttpPost]
        [ActionName("add")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            user.Password = HashPassword(user.Password);
            User added = userDao.AddUser(user);
            if (added == default(User))
            {
                return BadRequest("Username already exists!");
            }
            return Ok(added);
        }

        [HttpPost]
        [ActionName("register")]
        [AllowAnonymous]
        public IHttpActionResult SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            if (user.Role != UserType.Buyer)
            {
                return BadRequest("Invalid user role. Please select Buyer!");
            }
            user.Password = HashPassword(user.Password);
            User added = userDao.AddUser(user);
            if (added == default(User))
            {
                return BadRequest("Username already exists!");
            }
            return Ok(added);
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            if (userDao.FindById(user.ID) == null)
            {
                return BadRequest("Selected user does not exist");
            }
            return Ok(userDao.UpdateUser(user));
        }
        [HttpDelete]
        [ActionName("delete")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = userDao.FindById(id);
            if (user == default(User))
            {
                return NotFound();
            }
            return Ok(userDao.DeleteUser(user.ID));
        }

        private string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(_salt);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInput = HashPassword(password);
            return string.Equals(hashedInput, hashedPassword);
        }

        public string GenerateJwtToken(User user)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddMinutes(30);


            var tokenHandler = new JwtSecurityTokenHandler();
            var baseAddress = ConfigurationManager.AppSettings["BaseAddress"];

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            });
            DateTime now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwt_secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: baseAddress, audience: baseAddress,
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

    }
}
