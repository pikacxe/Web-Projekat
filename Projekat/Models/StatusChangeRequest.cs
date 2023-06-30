using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class StatusChangeRequest
    {
        public int orderId { get; set; }
        public OrderStatus status { get; set; }
    }
}