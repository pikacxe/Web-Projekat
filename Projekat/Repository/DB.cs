using System;
using System.Collections.Generic;
using Projekat.Models;

namespace Projekat.Repository
{
    public class DB
    {
        public static List<User> UsersList { get; set; } = new List<User>();
        public static List<Review> ReviewsList { get; set; } = new List<Review>();
        public static List<Product> ProductsList { get; set; } = new List<Product>();
        public static List<Order> OrdersList { get; set; } = new List<Order>();
        public static Dictionary<string, string> LoggedIn { get; set; } = new Dictionary<string, string>();
        public static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        } 
    }
}