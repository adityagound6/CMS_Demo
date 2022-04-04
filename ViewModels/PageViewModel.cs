using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_Demo.ViewModels
{
    public class PageViewModel
    {
        [Key]
        public int PageId { get; set; }
        [Required]
        public string PageName { get; set; }
        [AllowHtml]
        [Required]
        public string Description { get; set; }
        public bool Status { get; set; }
       
    }
}
