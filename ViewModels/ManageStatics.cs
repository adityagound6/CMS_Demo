using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class ManageStatics
    {
        public string Footer { get; set; }
        public IFormFile SiteLogo { get; set; }
    }
}
