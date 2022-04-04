using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class AddPageViewModel
    {
        [Required]
        public string PageName { get; set; }
        [Required]
        public string Description { get; set; }
        
    }
}
