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
        [Required(ErrorMessage = "Please Enter a valid page Name")]
        [Remote(action: "IsPageInUsed", controller: "Admin")]
        public string PageName { get; set; }
        [Required(ErrorMessage="Please Enter a valid page Description")]
        public string Description { get; set; }
        [Display(Name ="Page Status")]
        public bool Status { get; set; }
    }
}
