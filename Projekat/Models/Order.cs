using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public enum ProductStatus { ACTIVE, COMPLETED, CANCELED}
    public class Order
    {
        public int ID { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public User Buyer { get; set; }
        public DateTime OrderDate { get; set; }
        public ProductStatus Status { get; set; }

    }
}