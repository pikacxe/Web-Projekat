using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.DAO;
using Projekat.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Projekat.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        IDao<Order> orderDAO = new OrderDAO();
        IDao<Product> productDAO = new ProductDAO();

        [HttpGet]
        [ActionName("all")]
        public IHttpActionResult GetAllOrders()
        {
            if (!User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }
            return Ok(DB.OrdersList.Where(x => !x.isDeleted));
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Order found = orderDAO.FindByID(id);
            if (found == default(Order))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpGet]
        [ActionName("for-user")]
        public IHttpActionResult GetByUser(int id)
        {
            return Ok(DB.OrdersList.Where(x=> x.Buyer == id && !x.isDeleted));
        }

        [HttpPost]
        [ActionName("add")]
        [Authorize(Roles ="Administrator,Buyer")]
        public IHttpActionResult AddOrder(Order order)
        {
            if(order == default(Order))
            {
                return BadRequest("Invalid data!");
            }
            string message = ValidateOrder(order);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            Product toBuy = productDAO.FindByID(order.Product);
            if(toBuy == default(Product))
            {
                return NotFound();
            }
            if(toBuy.Amount - order.Amount < 0)
            {
                return BadRequest("Not enough product is available!");
            }
            toBuy.Amount -= order.Amount;
            order.OrderDate = DateTime.Now;
            order.Status = ProductStatus.ACTIVE;
            return Ok(orderDAO.Add(order));
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateOrder(Order order)
        {
            string message = ValidateOrder(order);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }

            if (orderDAO.FindByID(order.ID) == null)
            {
                return BadRequest("Selected Order does not exist");
            }
            return Ok(orderDAO.Update(order));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = orderDAO.FindByID(id);
            if (order == default(Order))
            {
                return NotFound();
            }
            return Ok(orderDAO.Delete(order.ID));
        }

        [HttpPut]
        [ActionName("delivered")]
        public IHttpActionResult OrderDelivered(int id)
        {
            Order order = orderDAO.FindByID(id);
            order.Status = ProductStatus.COMPLETED;
            return Ok("Order status updated");
        }

        private string ValidateOrder(Order order)
        {
            string message = string.Empty;
            if (order == default(Order))
            {
                message += "Provided data is invalid! ";
            }
            if (order.Product <= 0)
            {
                message += "Product is required! ";
            }
            if (order.Buyer <= 0)
            {
                message += "Buyer is required! ";
            }
            if (order.Amount <= 0)
            {
                message += "Amount must be greater than zero! ";
            }
            return message;
        }

    }
}
