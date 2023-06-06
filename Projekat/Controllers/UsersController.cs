using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Projekat.Models;
using Projekat.DataBase;

namespace Projekat.Controllers
{
    public class UsersController : ApiController
    {
        UserDAO userDAO = new UserDAO();

        [HttpGet]
        public ICollection<User> GetAllUsers()
        {
            return DB.UsersList.Values;
        }
        [HttpGet]
        public User GetByUsername(string username)
        {
            return userDAO.FindByUsername(username);
        }
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (user.FirstName == null || user.FirstName.Length == 0)
            {
                return BadRequest();
            }
            if (user.LastName == null || user.LastName.Length == 0)
            {
                return BadRequest();
            }
            return Ok(userDAO.AddUser(user));
        }
        [HttpPut]
        public IHttpActionResult UpdateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Provided data is invalid");
            }
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                return BadRequest("Firstname is required");
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                return BadRequest("Lastname is required");
            }
            if (userDAO.FindByUsername(user.Username) == null)
            {
                return BadRequest("Selected user does not exist");
            }
            return Ok(userDAO.UpdateUser(user));
        }
        [HttpDelete]
        public IHttpActionResult DeleteUser(string username)
        {
            User user = userDAO.FindByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(userDAO.RemoveUser(user.Username));
        }
    }
}
