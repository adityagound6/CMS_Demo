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
        public string PageName { get; set; }
        [Required(ErrorMessage="Please Enter a valid page Description")]
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
