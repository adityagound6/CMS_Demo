using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Please Enter a valid UserId")]
        [Display(Name = "User Id")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
    }
}
