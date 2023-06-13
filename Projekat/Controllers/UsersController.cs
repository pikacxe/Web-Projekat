using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat.Controllers
{
    public class UsersController : ApiController
    {
        string _salt = "sada/d.,asda;s,dl";
        IDao<User> userDAO = new UserDAO();

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return DB.UsersList.Where(x=> !x.isDeleted);
        }

        [HttpGet]
        [ActionName("username")]
        public User GetByUsername(string username)
        {
            return DB.UsersList.Find(x => x.Username == username && !x.isDeleted);
        }

        [HttpGet]
        [ActionName("login")]
        public IHttpActionResult LogIn(string username, string password)
        {
            User current = GetByUsername(username);
            if (current == default(User))
            {
                return BadRequest("Username is not in use.");
            }
            if (VerifyPassword(password, current.Password))
            {
                // TODO: Implement token auth 
                return Ok("User logged in");
            }
            return BadRequest("Invalid password!");
        }

        [HttpPost]
        [ActionName("signup")]
        public IHttpActionResult SignUp(User user)
        {
            string message = ValidateUser(user);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }
            user.Password = HashPassword(user.Password);
            User added = userDAO.Add(user);
            if(added == default(User))
            {
                return BadRequest("Username already exists!");
            }
            return Ok(added);
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateUser(User user)
        {
            if (userDAO.FindByID(user.ID) == null)
            {
                return BadRequest("Selected user does not exist");
            }
            string message = ValidateUser(user);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(userDAO.Update(user));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = userDAO.FindByID(id);
            if (user == default(User))
            {
                return NotFound();
            }
            return Ok(userDAO.Delete(user.ID));
        }

        private string ValidateUser(User user)
        {
            string message = string.Empty;
            if (user == null)
            {
                return "Invalid data!";
            }
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                message += "Firstname is required! ";
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                message += "Lastname is required! ";
            }
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                message += "Username is required! ";
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                message+= "Password is required! ";
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                message += "Email is required! ";
            }
            return message;
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
    }
}
