using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Projekat.Controllers
{
    [Authorize(Roles ="Administrator,Buyer")]
    public class OrdersController : ApiController
    {
        IOrderRepository orderRepo = new OrderRepository();

        [HttpGet]
        [ActionName("all")]
        [Authorize(Roles ="Administrator")]
        public IHttpActionResult GetAllOrders()
        {
            return Ok(orderRepo.GetAll());
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Order found = orderRepo.FindById(id);
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
            return Ok(orderRepo.FindByUser(id));
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string message;
            Order added = orderRepo.AddOrder(order, out message);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(added);
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string message;
            Order updated = orderRepo.UpdateOrder(order, out message);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(updated);
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order deleted = orderRepo.DeleteOrder(id);
            if(deleted == default(Order))
            {
                return NotFound();
            }
            return Ok(deleted);
        }

        [HttpPut]
        [ActionName("delivered")]
        public IHttpActionResult OrderDelivered(int id)
        {
            string result = orderRepo.UpdateOrderStatus(id, OrderStatus.COMPLETED);
            if(result == string.Empty)
            {
                return InternalServerError();
            }
            return Ok(result);
        }
    }
}
