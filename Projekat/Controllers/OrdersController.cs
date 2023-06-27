using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;

namespace Projekat.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        IOrderRepository orderRepo = new OrderRepository();
        IOrderDao orderDao = new OrderDAO();
        IProductDao productDao = new ProductDAO();

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
            Order found = orderDao.FindById(id);
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Product toBuy = productDao.FindById(order.Product);
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
            order.Status = OrderStatus.ACTIVE;
            return Ok(orderDao.AddOrder(order));
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (orderDao.FindById(order.ID) == null)
            {
                return BadRequest("Selected Order does not exist");
            }
            return Ok(orderDao.UpdateOrder(order));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = orderDao.FindById(id);
            if (order == default(Order))
            {
                return NotFound();
            }
            return Ok(orderDao.DeleteOrder(order.ID));
        }

        [HttpPut]
        [ActionName("delivered")]
        public IHttpActionResult OrderDelivered(int id)
        {
            string result = orderRepo.UpdateOrderStatus(id, OrderStatus.COMPLETED);
            return Ok(result);
        }
    }
}
