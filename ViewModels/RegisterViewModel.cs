using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CMS_Demo.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        //[Remote(action: "IsEmailInUse", controller: "Admin")]
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUsed", controller: "Admin")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="password and confirm password must be match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
