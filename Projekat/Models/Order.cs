using System;

namespace Projekat.Models
{
    public enum ProductStatus { ACTIVE, COMPLETED, CANCELED}
    public class Order
    {
        public int ID { get; set; }
        public int Product { get; set; }
        public int Amount { get; set; }
        public int Buyer { get; set; }
        public DateTime OrderDate { get; set; }
        public ProductStatus Status { get; set; }
        public bool isDeleted { get; set; }
    }
}