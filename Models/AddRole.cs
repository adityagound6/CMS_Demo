using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Models
{
    public class AddRole
    {
        [Key]
        public int RoleId { get; set; }
        
        public string RoleName { get; set; }
    }
}
