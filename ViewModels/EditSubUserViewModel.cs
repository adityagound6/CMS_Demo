using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.ViewModels
{
    public class EditSubUserViewModel : AddSubUserViewModel
    {
        public int UserId { get; set; }
        public bool isSelected { get; set; }
    }
}
