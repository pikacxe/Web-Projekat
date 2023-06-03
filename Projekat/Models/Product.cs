using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Amout { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string PublishDate { get; set; }
        public string City { get; set; }
        public List<Review> Review { get; set; }
        public bool isAvailable { get; set; }
    }
}