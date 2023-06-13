using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public class ProductDAO : IDao<Product>
    {
        public Product FindByID(int id)
        {
            return DB.ProductsList.Find(x => x.ID == id && !x.isDeleted);
        }

        public Product Add(Product product)
        {
            product.ID = DB.GenerateId();
            DB.ProductsList.Add(product);
            return product;
        }

        public Product Update(Product product)
        {
            Product old = FindByID(product.ID);
            if (old != default(Product))
            {
                old.Title = product.Title;
                old.Price = product.Price;
                old.Amount = product.Amount;
                old.Description = product.Description;
                old.Image = product.Image;
                old.PublishDate = product.PublishDate;
                old.City = product.City;
                old.isAvailable = product.isAvailable;
            }
            return old;
        }
        public Product Delete(int id)
        {
            Product deleted = FindByID(id);
            if(deleted != default(Product))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }
    }
}