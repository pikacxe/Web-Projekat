using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order FindById(int id);
        IEnumerable<Order> FindByUser(int userId);

        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="order">New order object</param>
        /// <param name="message">Validation error message</param>
        /// <returns>Added order if there was no validation error
        /// otherwise default(Order)</returns>
        Order AddOrder(Order order, out string message);

        /// <summary>
        /// Update existing order
        /// </summary>
        /// <param name="order">New order object</param>
        /// <param name="message">Validation error message</param>
        /// <returns>Updated order if there was no validation error
        /// otherwise default(Order)</returns>
        Order UpdateOrder(Order updatedOrder, out string message);
        Order DeleteOrder(int id);

        /// <summary>
        /// Updates order status
        /// </summary>
        /// <param name="orderId">Id of the order to be updated</param>
        /// <param name="status">Updated order status</param>
        /// <returns>String representation of updated status</returns>
        string UpdateOrderStatus(int orderId, OrderStatus status);
    }
}
