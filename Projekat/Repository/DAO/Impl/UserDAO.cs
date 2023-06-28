using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository.DAO.Impl
{
    public class UserDAO : IUserDao
    {
        public User FindById(int id)
        {
            return DB.UsersList.Find(x => x.ID == id && !x.isDeleted);
        }
        public IEnumerable<User> GetAll()
        {
            return DB.UsersList.FindAll(x => !x.isDeleted);
        }

        public User FindByUsername(string username)
        {
            return DB.UsersList.Find(x => x.Username == username && !x.isDeleted);
        }

        public User AddUser(User user)
        {
            if (DB.UsersList.Find(x => x.Username == user.Username && !x.isDeleted) == default(User))
            {
                user.ID = DB.GenerateId();
                DB.UsersList.Add(user);
                return user;
            }
            return default(User);
        }

        public User DeleteUser(int id)
        {
            User deleted = FindById(id);
            if (deleted != default(User))
            {
                deleted.isDeleted = true;
                return deleted;
            }
            return deleted;
        }

        public User UpdateUser(User updatedUser)
        {
            User existingUser = FindById(updatedUser.ID);
            if (existingUser != default(User))
            {
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Gender = updatedUser.Gender;
                existingUser.Email = updatedUser.Email;
                existingUser.DateOfBirth = updatedUser.DateOfBirth;
            }
            return existingUser;
        }
    }
}