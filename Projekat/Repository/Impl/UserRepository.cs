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
    }
}