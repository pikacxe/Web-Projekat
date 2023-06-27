using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public interface IProductDao
    {
        IEnumerable<Product> GetAll();
        Product FindById(int id);
        IEnumerable<Product> FindByIds(List<int> ids);
        Product AddProduct(Product product);
        Product UpdateProduct(Product updatedProduct);
        Product DeleteProduct(int product);
    }
}
