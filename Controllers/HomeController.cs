using CMS_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Controllers
{
    public class HomeController : Controller
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
        [HttpPost]
        public IActionResult Index(string Email, string Password)
        {
            try
            {
                var user = _con.Users.Where(s => s.Email == Email & s.Password == Password);
                if (user == null)
                {
                    return View();
                }
                return View("Home");
            }
            catch(DbException)
            {
                return View();
            }
        }
    }
}
