using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository.DAO.Impl
{
    public class OrderDAO : IOrderDao
    {
        public Order AddOrder(Order item)
        {
            item.ID = DB.GenerateId();
            DB.OrdersList.Add(item);
            return item;
        }

        public Order DeleteOrder(int id)
        {
            Order deleted = FindById(id);
            if(deleted != default(Order))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }

        public Order FindById(int id)
        {
            return DB.OrdersList.Find(x => x.ID == id && !x.isDeleted);
        }

        public IEnumerable<Order> FindByUser(int userId)
        {
            return DB.OrdersList.FindAll(x => x.Buyer == userId && !x.isDeleted);
        }

        public IEnumerable<Order> GetAll()
        {
            return DB.OrdersList.FindAll(x => !x.isDeleted);
        }

        public Order UpdateOrder(Order updatedOrder)
        {
            Order old = FindById(updatedOrder.ID);
            if(old != default(Order))
            {
                old.Product = updatedOrder.Product;
                old.Amount = updatedOrder.Amount;
                old.Buyer = updatedOrder.Buyer;
                old.OrderDate = updatedOrder.OrderDate;
            }
            return old;
        }
    }
}