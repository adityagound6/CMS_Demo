using CMS_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Controller
{
    public class HomeController
    {
        private readonly AppDbContext _con;

        public HomeController(AppDbContext con)
        {
            _con = con;
        }
    }
}
