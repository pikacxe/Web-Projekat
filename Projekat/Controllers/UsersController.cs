using System;
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

namespace Projekat.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        string _salt = ConfigurationManager.AppSettings["PasswdSalt"];
        string jwt_secret = ConfigurationManager.AppSettings["JwtSecretKey"];
        IDao<User> userDAO = new UserDAO();

        [HttpGet]
        [ActionName("all")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult GetAllUsers()
        {
            return Ok(DB.UsersList.Where(x => !x.isDeleted));
        }
        private User GetByUsername(string username)
        {
            return DB.UsersList.Find(x => x.Username == username && !x.isDeleted);
        }

        [HttpGet]
        [ActionName("current")]
        public IHttpActionResult GetUsername()
        {
            User current = GetByUsername(User.Identity.Name);
            return Ok(new { id = current.ID, name = current.Username, role = current.Role.ToString() });
        }

        [HttpPost]
        [ActionName("login")]
        [AllowAnonymous]
        public IHttpActionResult LogIn([FromBody] LoginRequest req)
        {
            User current = GetByUsername(req.username);
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
            string message = ValidateUser(user);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            user.Password = HashPassword(user.Password);
            User added = userDAO.Add(user);
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
            string message = ValidateUser(user);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            if (user.Role != UserType.Buyer)
            {
                return BadRequest("Invalid user role. Please select Buyer!");
            }
            user.Password = HashPassword(user.Password);
            User added = userDAO.Add(user);
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
        [Authorize(Roles = "Administrator")]
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
                message += "Password is required! ";
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
