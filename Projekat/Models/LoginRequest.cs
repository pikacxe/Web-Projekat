using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings=false,ErrorMessage ="Invalid username!")]
        public string username { get; set; }

        [Required(AllowEmptyStrings=false,ErrorMessage ="Invalid password!")]
        public string password { get; set; }
    }
}