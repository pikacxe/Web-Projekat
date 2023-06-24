using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public class OrderDAO : IDao<Order>
    {
        public Order Add(Order item)
        {
            item.ID = DB.GenerateId();
            DB.OrdersList.Add(item);
            return item;
        }

        public Order Delete(int id)
        {
            Order deleted = FindByID(id);
            if(deleted != default(Order))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }

        public Order FindByID(int id)
        {
            return DB.OrdersList.Find(x => x.ID == id && !x.isDeleted);
        }

        public Order Update(Order updated)
        {
            Order old = FindByID(updated.ID);
            if(old != default(Order))
            {
                old.Product = updated.Product;
                old.Amount = updated.Amount;
                old.Buyer = updated.Buyer;
                old.OrderDate = updated.OrderDate;
                old.Status = updated.Status;
            }

            return updated;
        }
    }
}