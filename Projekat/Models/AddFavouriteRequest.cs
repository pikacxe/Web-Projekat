using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class AddFavouriteRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}