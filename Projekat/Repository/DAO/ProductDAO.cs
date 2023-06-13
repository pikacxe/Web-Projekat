using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public class ProductDAO
    {
        public Product FindById(int id)
        {
            if (!DB.ProductsList.ContainsKey(id) || DB.ProductsList[id].isDeleted)
            {
                return null;
            }
            return DB.ProductsList[id];
        }

        public Product AddProduct(Product product)
        {
            product.ID = DB.GenerateId();
            DB.ProductsList.Add(product.ID, product);
            return product;
        }

        public Product UpdateProduct(Product product)
        {
            if (!DB.ProductsList.ContainsKey(product.ID) || DB.ProductsList[product.ID].isDeleted)
            {
                return null;
            }
            Product old = DB.ProductsList[product.ID];
            old.Title = product.Title;
            old.Description = product.Description;
            old.City = product.City;
            old.Amout = product.Amout;
            old.Price = product.Price;
            old.Image = product.Image;
            old.PublishDate = product.PublishDate;
            return product;
        }
        public Product DeleteProduct(int id)
        {
            if (!DB.ProductsList.ContainsKey(id) || DB.ProductsList[id].isDeleted)
            {
                return null;
            }
            Product deleted = DB.ProductsList[id];
            deleted.isDeleted = true;
            return deleted;
        }
    }
}