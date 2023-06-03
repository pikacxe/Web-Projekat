using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Projekat.Models;

namespace Projekat.Controllers
{
    public class UsersController : ApiController
    {
        public IEnumerable<User> Get()
        {
            return Users.UsersList.Values.ToList();
        }
        public User Get(int id)
        {
            return Users.FindById(id);
        }
        public IHttpActionResult Post(User user)
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
            return Ok(Users.AddUser(user));
        }
        public IHttpActionResult Put(User user)
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
            if (Users.FindById(user.ID) == null)
            {
                return BadRequest();
            }
            return Ok(Users.UpdateUser(user));
        }
        public IHttpActionResult Delete(int id)
        {
            User user = Users.FindById(id);
            if (user == null)
            {
                return NotFound();
            }
            Users.RemoveUser(user.Username);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
