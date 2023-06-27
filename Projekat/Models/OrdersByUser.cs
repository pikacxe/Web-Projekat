using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class OrdersByUser
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string BuyerName { get; set; }
        public int BuyerID { get; set; }
        public int Amount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string StatusMessage { get; set; }
    }
}