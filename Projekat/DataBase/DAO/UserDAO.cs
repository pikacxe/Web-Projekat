using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.DataBase
{
    public class UserDAO
    {
        public User FindByUsername(string username)
        {
            return DB.UsersList[username];
        }

        public User AddUser(User user)
        {
            if (DB.UsersList.ContainsKey(user.Username) && !DB.UsersList[user.Username].isDeleted)
            {
                return null;
            }
            DB.UsersList.Add(user.Username, user);
            return user;
        }

        public User RemoveUser(string userName)
        {
            User deleted = DB.UsersList[userName];
            deleted.isDeleted = true;
            return deleted;
        }

        public User UpdateUser(User user)
        {
            if (DB.UsersList.ContainsKey(user.Username) && !DB.UsersList[user.Username].isDeleted)
            {
                return null;
            }
            User existinUser = DB.UsersList[user.Username];
            existinUser.FirstName = user.FirstName;
            existinUser.LastName = user.LastName;
            existinUser.Gender = user.Gender;
            existinUser.Email = user.Email;
            existinUser.DateOfBirth = user.DateOfBirth;

            return existinUser;
        }

        public void ChangeRole(string username, UserType newRole)
        {
            DB.UsersList[username].Role = newRole;
        }
    }
}