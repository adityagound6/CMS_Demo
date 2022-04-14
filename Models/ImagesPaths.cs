using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Models
{
    public class ImagesPaths
    {
        [Key]
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}
