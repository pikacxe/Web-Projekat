using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;

namespace Projekat.Repository.Impl
{
    public class OrderRepository: IOrderRepository
    {
        IOrderDao orderDao = new OrderDAO();
        IUserDao userDao = new UserDAO();
        IProductDao productDao = new ProductDAO();

        public IEnumerable<Order> GetAll()
        {
            return orderDao.GetAll();
        }
        public Order FindById(int id)
        {
            return orderDao.FindById(id);
        }

        public IEnumerable<Order> FindByUser(int userId)
        {
            User user = userDao.FindById(userId);
            if (user == default(User))
            {
                return Enumerable.Empty<Order>();
            }
            return orderDao.FindByUser(userId);
        }
        public Order AddOrder(Order order, out string message)
        {
            message = string.Empty;
            Product product = productDao.FindById(order.Product);
            if(product == default(Product))
            {
                message = "Product does not exist!";
                return default(Order);
            }
            User buyer = userDao.FindById(order.Buyer);
            if(buyer == default(User))
            {
                message = "Invalid buyer!";
                return default(Order);
            }
            if(product.Amount - order.Amount < 0)
            {
                message = "Not enough product is available";
                return default(Order);
            }
            product.Amount -= order.Amount;
            order.Status = OrderStatus.ACTIVE;
            order.OrderDate = DateTime.UtcNow;

            return orderDao.AddOrder(order);
        }

        public Order DeleteOrder(int id)
        {
            Order order = orderDao.FindById(id);
            if(order == default(Order))
            {
                return order;
            }
            Product product = productDao.FindById(id);
            product.Amount += order.Amount;
            return orderDao.DeleteOrder(id);
        }

        public Order UpdateOrder(Order updatedOrder, out string message)
        {
            message = string.Empty;
            Order old = orderDao.FindById(updatedOrder.ID);
            if(old == default(Order))
            {
                message = "Order does not exist!";
                return old;
            }

            Product product = productDao.FindById(updatedOrder.Product);
            if (product == default(Product))
            {
                message = "Product does not exist!";
                return default(Order);
            }

            User buyer = userDao.FindById(updatedOrder.Buyer);
            if (buyer == default(User))
            {
                message = "Invalid buyer!";
                return default(Order);
            }

            // Return old product amount before checking again
            product.Amount += old.Amount;
            if (product.Amount - updatedOrder.Amount < 0)
            {
                message = "Not enough product is available";
                return default(Order);
            }
            product.Amount -= updatedOrder.Amount;
            updatedOrder.Status = OrderStatus.ACTIVE;
            updatedOrder.OrderDate = DateTime.UtcNow;
            return orderDao.UpdateOrder(updatedOrder);
        }
        public string UpdateOrderStatus(int orderId, OrderStatus status)
        {
            Order order = orderDao.FindById(orderId);
            if (order == default(Order))
            {
                return string.Empty;
            }
            if(status == OrderStatus.CANCELED && order.Status == OrderStatus.ACTIVE)
            {
                Product product = productDao.FindById(order.Product);
                if(product != default(Product))
                {
                    product.Amount += order.Amount;
                }
            }
            order.Status = status;
            return status.ToString();
        }

    }
}