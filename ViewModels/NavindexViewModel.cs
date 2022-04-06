using CMS_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class NavindexViewModel : AddPage
    {
        public List<Nav_List> NavPages {get;set;}
    }
    public class Nav_List 
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public int SubPageId { get; set; }
       
    }
}
