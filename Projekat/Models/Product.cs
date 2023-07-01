using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Projekat.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required(AllowEmptyStrings =false,
            ErrorMessage ="Seller is invalid")]
        public int SellerId { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Name is invalid !")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Price is invalid !")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Amount is invalid !")]
        public int Amount { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Description is invalid !")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Image path is invalid !")]
        public string Image { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "City is invalid !")]
        public string City { get; set; }
        public DateTime? PublishDate { get; set; }

        public List<int> Reviews { get; set; } = new List<int>();
        public bool isAvailable { get
            {
                return Amount > 0;
            }
        }
        public bool isDeleted { get; set; }
    }
}