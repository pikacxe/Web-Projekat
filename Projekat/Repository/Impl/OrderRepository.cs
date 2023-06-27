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



        public string UpdateOrderStatus(int orderId, OrderStatus status)
        {
            Order order = orderDao.FindById(orderId);
            if (order == default(Order))
            {
                return string.Empty;
            }
            order.Status = status;
            return status.ToString();
        }

    }
}