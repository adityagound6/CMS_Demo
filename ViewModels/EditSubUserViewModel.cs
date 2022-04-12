using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_Demo.Models;

namespace CMS_Demo.ViewModels
{
    public class EditSubUserViewModel : AddSubUserViewModel
    {
        public EditSubUserViewModel()
        {
            isActive = new Dictionary<int, bool>();
            AddRole = new Dictionary<int, string>();
        }
        public int UserId { get; set; }
        public Dictionary<int,bool> isActive { get; set; }
        public Dictionary<int,string> AddRole { get; set; }
    }
}
