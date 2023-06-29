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
            if (user == default(User))
            {
                message = "User not found!";
                return Enumerable.Empty<Product>();
            }
            if (user.Role == UserType.Buyer)
            {
                return productDao.FindByIds(user.Favourites);
            }
            else if (user.Role == UserType.Seller)
            {
                return productDao.FindByIds(user.PublishedProducts);
            }
            return Enumerable.Empty<Product>();
        }
        public Product AddProduct(Product product, out string message)
        {
            message = string.Empty;
            User seller = userDao.FindById(product.SellerId);
            if (seller == default(User))
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

        public IEnumerable<int> AddProductToFavourites(int userId, int productId, out string message)
        {
            message = string.Empty;
            User user = userDao.FindById(userId);
            if (user == default(User))
            {
                message = "User not found!";
                return Enumerable.Empty<int>();
            }
            Product product = productDao.FindById(productId);
            if (product == default(Product))
            {
                message = "Product not found";
                return Enumerable.Empty<int>();
            }
            if (user.Favourites.Contains(productId))
            {
                user.Favourites.Remove(productId);
            }
            else
            {
                user.Favourites.Add(productId);
            }
            return user.Favourites;
        }

        public Product DeleteProduct(int productId)
        {
            Product product = productDao.FindById(productId);
            if(product == default(Product))
            {
                return product;
            }
            User user = userDao.FindById(product.SellerId);
            user.PublishedProducts.Remove(productId);
            userDao.RemoveProductFromFav(productId);
            return productDao.DeleteProduct(productId);
        }

        public Product UpdateProduct(Product updatedProduct, out string message)
        {
            message = string.Empty;
            if(productDao.FindById(updatedProduct.ID) == default(Product))
            {
                message = "Product not found";
                return default(Product);
            }
            User seller = userDao.FindById(updatedProduct.SellerId);
            if (seller == default(User))
            {
                message = "Seller not found";
                return updatedProduct;
            }
            if(updatedProduct.Price <= 0)
            {
                message = "Price must be greater than 0";
                return default(Product);
            }
            if(updatedProduct.Amount < 0)
            {
                message = "Amount can not be negative";
                return default(Product);
            }
            // TODO: Check City against a list of available cities
            updatedProduct.PublishDate = DateTime.UtcNow;
            Product updated = productDao.UpdateProduct(updatedProduct);

            return updated;
        }
    }
}