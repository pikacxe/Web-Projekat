using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public interface IOrderDao
    {
        IEnumerable<Order> GetAll();
        Order FindById(int id);
        IEnumerable<Order> FindByUser(int id);
        Order AddOrder(Order order);
        Order UpdateOrder(Order updatedOrder);
        Order DeleteOrder(int id);
    }
}
