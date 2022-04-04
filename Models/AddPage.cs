using CMS_Demo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_Demo.Models
{
    public class AddPage 
    {
        [Key]
        public int PageId { get; set; }
        [Required]
        public string PageName { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
