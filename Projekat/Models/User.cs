using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public enum UserType { Buyer, Seller, Administrator }
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public UserType Role { get; set; }
        public List<int> Orders { get; set; }
        public List<int> Favourites { get; set; }
        public List<int> PublishedProducts { get; set; }
        public bool isDeleted { get; set; }
    }
}