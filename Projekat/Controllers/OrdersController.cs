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
    public class OrdersController : ApiController
    {
        IDao<Order> orderDAO = new OrderDAO();

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<Order> GetAllOrders()
        {
            return DB.OrdersList.Where(x => !x.isDeleted);
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

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddOrder(Order order)
        {
            string message = ValidateOrder(order);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
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
            if (order == null)
            {
                return NotFound();
            }
            return Ok(orderDAO.Delete(order.ID));
        }

        private string ValidateOrder(Order order)
        {
            string message = string.Empty;
            if (order == null)
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
