using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;
using Projekat.Models;

namespace Projekat.Repository
{
    public class ProductRepository : IProductRepository
    {
        IProductDao productDAO = new ProductDAO();
        IUserDao userDAO = new UserDAO();

    }
}