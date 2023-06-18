using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class ProductsController : ApiController
    {
        IDao<Product> productDAO = new ProductDAO();

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<Product> GetAllProducts()
        {
            return DB.ProductsList.Where(x => !x.isDeleted);
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Product found = productDAO.FindByID(id);
            if(found == default(Product))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddProduct(Product product)
        {
            string message = ValidateProduct(product);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(productDAO.Add(product));
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateProduct(Product product)
        {
            string message = ValidateProduct(product);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }

            if (productDAO.FindByID(product.ID) == null)
            {
                return BadRequest("Selected Product does not exist");
            }
            return Ok(productDAO.Update(product));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = productDAO.FindByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(productDAO.Delete(product.ID));
        }

        private string ValidateProduct(Product product)
        {
            string message = string.Empty;
            if (product == null)
            {
                message += "Provided data is invalid! ";
            }
            if (string.IsNullOrWhiteSpace(product.Title))
            {
                message += "Title is required! ";
            }
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                message += "Description is required! ";
            }
            if (string.IsNullOrWhiteSpace(product.Image))
            {
                message += "Image is required! ";
            }
            if (string.IsNullOrWhiteSpace(product.PublishDate))
            {
                message += "Publish date is required! ";
            }
            if (string.IsNullOrWhiteSpace(product.City))
            {
                message += "City is required! ";
            }
            if(product.Price < 0)
            {
                message += "Price must be greater or equal zero! ";
            }
            if(product.Amount <= 0)
            {
                message += "Amount must be greater than zero! ";
            }
            return message;
        }

    }
}
