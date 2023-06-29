using System;
using System.ComponentModel.DataAnnotations;

namespace Projekat.Models
{
    public enum OrderStatus { ACTIVE, COMPLETED, CANCELED}
    public class Order
    {
        public int ID { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Product is invalid !")]
        public int Product { get; set; }

        public string ProductName { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Amount is invalid !")]
        public int Amount { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Buyer is invalid !")]
        public int Buyer { get; set; }

        public DateTime? OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusMessage { get => Status.ToString(); }
        public bool isDeleted { get; set; }
    }
}