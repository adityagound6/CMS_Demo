using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class AddPageViewModel
    {
        [Required(ErrorMessage = "Please enter a valid Page Name.")]
        [Remote(action: "IsPageInUsed", controller: "Admin")]
        public string PageName { get; set; }
        [Required(ErrorMessage="Please enter a valid Page Description.")]
        public string Description { get; set; }
        [Display(Name ="Page Status")]
        public bool Status { get; set; }
    }
}
