using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository.DAO.Impl
{
    public class ProductDAO : IProductDao
    {

        public IEnumerable<Product> GetAll()
        {
            return DB.ProductsList.FindAll(p => !p.isDeleted);
        }
        public Product FindById(int id)
        {
            return DB.ProductsList.Find(x => x.ID == id && !x.isDeleted);
        }

        public Product AddProduct(Product product)
        {
            product.ID = DB.GenerateId();
            DB.ProductsList.Add(product);
            return product;
        }

        public Product UpdateProduct(Product updatedProduct)
        {
            Product old = FindById(updatedProduct.ID);
            if (old != default(Product))
            {
                old.Title = updatedProduct.Title;
                old.Price = updatedProduct.Price;
                old.Amount = updatedProduct.Amount;
                old.Description = updatedProduct.Description;
                old.Image = updatedProduct.Image;
                old.PublishDate = updatedProduct.PublishDate;
                old.City = updatedProduct.City;
            }
            return old;
        }
        public Product DeleteProduct(int id)
        {
            Product deleted = FindById(id);
            if(deleted != default(Product))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }
        public IEnumerable<Product> FindByIds(List<int> ids)
        {
            return DB.ProductsList.FindAll(p => ids.Contains(p.ID));
        }
    }
}