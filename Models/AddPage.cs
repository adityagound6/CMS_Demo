using CMS_Demo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Models
{
    public class AddPage : AddPageViewModel
    {
        [Key]
        public int PageId { get; set; }
    }
}
