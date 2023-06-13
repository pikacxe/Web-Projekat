using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository
{
    public class DB
    {
        public static Dictionary<int, User> UsersList { get; set; } = new Dictionary<int, User>();
        public static Dictionary<int, Review> ReviewsList { get; set; } = new Dictionary<int, Review>();
        public static Dictionary<int, Product> ProductsList { get; set; } = new Dictionary<int, Product>();
        public static Dictionary<int, Order> OrdersList { get; set; } = new Dictionary<int, Order>();

        public static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        } 
    }
}