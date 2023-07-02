using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class ChangePasswordRequest
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Invalid user id")]
        public int UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid password")]
        public string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid password")]
        public string NewPassword { get; set; }
    }
}