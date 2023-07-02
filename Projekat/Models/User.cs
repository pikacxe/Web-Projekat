using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Projekat.Models
{
    public enum UserType
    {
        Buyer = 0,
        Seller = 1,
        Administrator = 2
    }
    public class User
    {
        public int ID { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Username is invalid!")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Password is invalid!")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "First name is invalid!")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Last name is invalid!")]
        public string LastName { get; set; }

        public string FullName { get => FirstName + " " + LastName; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Gender is invalid!")]
        public string Gender { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Email is invalid!")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Date of birth is invalid!")]
        public DateTime? DateOfBirth { get; set; }
        public UserType Role { get; set; }
        public string RoleName { get => Role.ToString(); }
        public List<int> Orders { get; set; } = new List<int>();
        public List<int> Favourites { get; set; } = new List<int>();
        public List<int> PublishedProducts { get; set; } = new List<int>();
        public bool isDeleted { get; set; }
    }
}