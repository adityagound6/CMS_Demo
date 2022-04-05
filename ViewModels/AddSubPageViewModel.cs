using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class AddSubPageViewModel :AddPageViewModel
    {
        public int PageId { get; set; }
        public int MainMenu { get; set; }
    }
}
