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
    [Authorize]
    public class ProductsController : ApiController
    {
        IProductRepository productRepo = new ProductRepository();
        IProductDao productDao = new ProductDAO();
        IUserDao userDao = new UserDAO();

        [HttpGet]
        [ActionName("all")]
        [AllowAnonymous]
        public IHttpActionResult GetAllProducts()
        {
            return Ok(DB.ProductsList.Where(x => !x.isDeleted));
        }

        [HttpGet]
        [ActionName("find")]
        [AllowAnonymous]
        public IHttpActionResult GetById(int id)
        {
            Product found = productDao.FindById(id);
            if(found == default(Product))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpPost]
        [ActionName("add")]
        [Authorize(Roles ="Administrator,Seller")]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            return Ok(productDao.AddProduct(product));
        }

        [HttpGet]
        [ActionName("seller")]
        [Authorize(Roles = "Administrator,Seller")]
        public IHttpActionResult GetProductsBySeller(int id)
        {
            User seller = userDao.FindById(id);
            return Ok();
        }

        [HttpPut]
        [ActionName("update")]
        [Authorize(Roles = "Administrator,Seller")]
        public IHttpActionResult UpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            if (productDao.FindById(product.ID) == null)
            {
                return BadRequest("Selected Product does not exist");
            }
            return Ok(productDao.UpdateProduct(product));
        }
        [HttpDelete]
        [ActionName("delete")]
        [Authorize(Roles = "Administrator,Seller")]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = productDao.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(productDao.DeleteProduct(product.ID));
        }

    }
}
