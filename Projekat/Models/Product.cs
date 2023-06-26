﻿using System.Collections.Generic;

namespace Projekat.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string PublishDate { get; set; }
        public string City { get; set; }
        public List<int> Review { get; set; }
        public bool isAvailable { get
            {
                return Amount > 0;
            }
        }
        public bool isDeleted { get; set; }
    }
}