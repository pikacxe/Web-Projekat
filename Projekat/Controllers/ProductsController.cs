using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;

namespace Projekat.Controllers
{
    [Authorize(Roles ="Administrator,Seller")]
    public class ProductsController : ApiController
    {
        IProductRepository productRepo = new ProductRepository();

        [HttpGet]
        [ActionName("all")]
        [AllowAnonymous]
        public IHttpActionResult GetAllProducts()
        {
            return Ok(productRepo.GetAll());
        }

        [HttpGet]
        [ActionName("find")]
        [AllowAnonymous]
        public IHttpActionResult GetById(int id)
        {
            Product found = productRepo.FindById(id);
            if (found == default(Product))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            string message;
            Product added = productRepo.AddProduct(product, out message);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(added);
        }

        [HttpGet]
        [ActionName("seller")]
        [Authorize(Roles ="Buyer,Seller")]
        public IHttpActionResult GetProductsBySeller(int id)
        {
            string message;
            var result = productRepo.GetProductByUser(id, out message);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateProduct(Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            string message;
            Product updated = productRepo.AddProduct(updatedProduct, out message);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(updated);
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = productRepo.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(productRepo.DeleteProduct(product.ID));
        }

    }
}
