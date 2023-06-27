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

        string UpdateOrderStatus(int orderId, OrderStatus status);
    }
}
