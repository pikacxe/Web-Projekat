using Projekat.Models;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        IUserDao userDao = new UserDAO();

        public User AddUser(User user)
        {
            return userDao.AddUser(user);
        }

        public User DeleteUser(int id)
        {
            return userDao.DeleteUser(id);
        }

        public User FindById(int id)
        {
            return userDao.FindById(id);
        }

        public User FindByUsername(string username)
        {
            return userDao.FindByUsername(username);
        }

        public IEnumerable<User> GetAll()
        {
            return userDao.GetAll();
        }

        public User UpdateUser(User updatedUser)
        {
            User user = userDao.UpdateUser(updatedUser);
            return user;
        }
    }
}