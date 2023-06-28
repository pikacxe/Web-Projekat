using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product FindById(int id);

        /// <summary>
        /// Find all products assoiated by user.
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="message"></param>
        /// <returns>User.Favourites if the user has role of Buyer or
        /// User.PublishedProducts if the user has role of Seller,
        /// otherwise if validation error occurs return Enumerable.Empty()</returns>
        IEnumerable<Product> GetProductByUser(int userId, out string message);
        
        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product">New product object</param>
        /// <param name="message">Validation error message</param>
        /// <returns>Added product if there was no validation error
        /// otherwise default(Product)</returns>
        Product AddProduct(Product product, out string message);

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product">Updated product object</param>
        /// <param name="message">Validation error message</param>
        /// <returns>Updated product if there was no validation error
        /// otherwise default(Product)</returns>
        Product UpdateProduct(Product updatedProduct, out string message);
        Product DeleteProduct(int productId);

    }
}
