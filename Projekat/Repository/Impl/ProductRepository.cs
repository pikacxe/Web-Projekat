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
        IProductDao productDao = new ProductDAO();
        IUserDao userDao = new UserDAO();
        public IEnumerable<Product> GetAll()
        {
            return productDao.GetAll();
        }

        public Product FindById(int id)
        {
            return productDao.FindById(id);
        }

        public IEnumerable<Product> GetProductByUser(int userId, out string message)
        {
            message = string.Empty;
            User user = userDao.FindById(userId);
            if(user == default(User))
            {
                message = "User not found!";
                return Enumerable.Empty<Product>();
            }
            if(user.Role == UserType.Buyer)
            {
                return productDao.FindByIds(user.Favourites);
            }
            else if(user.Role == UserType.Seller)
            {
                return productDao.FindByIds(user.PublishedProducts);
            }
            return Enumerable.Empty<Product>();
        }
        public Product AddProduct(Product product, out string message)
        {
            message = string.Empty;
            User seller = userDao.FindById(product.SellerId);
            if(seller  == default(User))
            {
                message = "Seller not found";
                return product;
            }
            // TODO: Check City against a list of available cities
            product.PublishDate = DateTime.UtcNow;
            Product added = productDao.AddProduct(product);
            seller.PublishedProducts.Add(product.ID);
            return added;
        }

        public Product DeleteProduct(int productId)
        {
            return productDao.DeleteProduct(productId);
        }

        public Product UpdateProduct(Product updatedProduct, out string message)
        {
            message = string.Empty;
            // TODO: Possible validation checks
            return productDao.UpdateProduct(updatedProduct);
        }
    }
}