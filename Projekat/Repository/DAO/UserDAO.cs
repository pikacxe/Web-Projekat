using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository
{
    public class UserDAO: IDao<User>
    {
        public User FindByID(int id)
        {
            if (Exists(id))
            {
                return DB.UsersList[id];
            }
            return null;
        }

        public User Add(User item)
        {
            if (!Exists(item.ID))
            {
                item.ID = DB.GenerateId();
                DB.UsersList.Add(item.ID, item);
                return item;
            }
            return null;
        }

        public User Delete(int id)
        {
            if (Exists(id))
            {
                DB.UsersList[id].isDeleted = true;
                return DB.UsersList[id];
            }
            return null;
        }

        public User Update(User updated)
        {
            User existinUser = DB.UsersList[updated.ID];
            existinUser.FirstName = updated.FirstName;
            existinUser.LastName = updated.LastName;
            existinUser.Gender = updated.Gender;
            existinUser.Email = updated.Email;
            existinUser.DateOfBirth = updated.DateOfBirth;

            return existinUser;
        }

        public void ChangeRole(int id, UserType newRole)
        {
            DB.UsersList[id].Role = newRole;
        }

        public bool Exists(int id)
        {
            if (DB.UsersList.ContainsKey(id) && !DB.UsersList[id].isDeleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}