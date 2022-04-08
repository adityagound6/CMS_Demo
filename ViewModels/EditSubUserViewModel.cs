using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class EditSubUserViewModel : AddSubUserViewModel
    {
        public EditSubUserViewModel()
        {
            isActive = new Dictionary<int, bool>();
        }
        public int UserId { get; set; }
        public Dictionary<int,bool> isActive { get; set; }
    }
}
