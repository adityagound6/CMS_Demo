using CMS_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Controllers
{
    public class HomeController:Controller
    {
        private readonly AppDbContext _con;

        public HomeController(AppDbContext con)
        {
            _con = con;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
