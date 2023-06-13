using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository
{
    public class UserDAO : IDao<User>
    {
        public User FindByID(int id)
        {
            return DB.UsersList.Find(x => x.ID == id && !x.isDeleted);
        }
        public User Add(User item)
        {
            if (DB.UsersList.Find(x => x.Username == item.Username && !x.isDeleted) == default(User))
            {
                item.ID = DB.GenerateId();
                DB.UsersList.Add(item);
                return item;
            }
            return default(User);
        }

        public User Delete(int id)
        {
            User deleted = FindByID(id);
            if (deleted != default(User))
            {
                deleted.isDeleted = true;
                return deleted;
            }
            return deleted;
        }

        public User Update(User updated)
        {
            User existinUser = FindByID(updated.ID);
            if (existinUser != default(User))
            {
                existinUser.FirstName = updated.FirstName;
                existinUser.LastName = updated.LastName;
                existinUser.Gender = updated.Gender;
                existinUser.Email = updated.Email;
                existinUser.DateOfBirth = updated.DateOfBirth;
            }
            return existinUser;
        }

    }
}