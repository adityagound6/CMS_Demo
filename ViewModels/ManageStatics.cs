using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class ManageStatics
    {
        public string Footer { get; set; }
        [Display(Name ="Site Logo")]
        public IFormFile SiteLogo { get; set; }
    }
}
