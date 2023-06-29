using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class CurrentUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public List<int> Favourites { get; set; }
    }
}