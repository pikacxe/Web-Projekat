using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class ChangeUsernameRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid user")]
        public int UserID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid username")]
        public string NewUsername { get; set; }
    }
}