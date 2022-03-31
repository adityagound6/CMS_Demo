using Microsoft.AspNetCore.Mvc;
using CMS_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_Demo.ViewModels;

namespace CMS_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }
            return View(model);
        }
    }
}
