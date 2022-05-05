using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class ManagePageViewModel : AddSubPageViewModel
    {
       public int PageId { get; set; }
        [Remote(action: "IsPageInUsed", controller: "Admin", AdditionalFields = nameof(PageName))]
        public string NewPageName { get; set; }
    }
}
