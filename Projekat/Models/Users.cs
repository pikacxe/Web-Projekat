using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Users
    {
        public static Dictionary<string, User> UsersList { get; set; } = new Dictionary<string, User>();


        public static User FindById(int id)
        {
            return UsersList.FirstOrDefault(x => x.Value.ID == id).Value;
        }

        public static User AddUser(User user)
        {
            if (UsersList.ContainsKey(user.Username))
            {
                return null;
            }
            user.ID = GenerateId();
            UsersList.Add(user.Username,user);
            return user;
        }

        public static bool RemoveUser(string userName)
        {
            return UsersList.Remove(userName);
        }

        public static User UpdateUser(User user)
        {
            User existinUser = FindById(user.ID);
            existinUser.FirstName = user.FirstName;
            existinUser.LastName = user.LastName;
            existinUser.Gender = user.Gender;
            existinUser.Email = user.Email;
            existinUser.DateOfBirth = user.DateOfBirth;

            return existinUser;
        }

        private static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        } 
    }
}