using CMS_Demo.Models;
using CMS_Demo.ViewModels;
using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            try
            {
                var user = _con.Users.Where(s => s.Email == Email & s.Password == Password).FirstOrDefault();
                if (user == null)
                {
                    return View();
                }
                HttpContext.Session.SetString("UserName", user.UserName);
                ViewBag.Session = HttpContext.Session.GetString("UserName");
                return RedirectToAction("index");
            }
            catch(DbException)
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Password = model.Password
                };
                _con.Users.Add(user);
                var status = _con.SaveChanges();
                if(status == 1)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    ViewBag.Session = HttpContext.Session.GetString("UserName");
                    return RedirectToAction("index");
                }
                return View();
            }
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AddPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPage(AddPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                AddPage pages = new AddPage
                {
                    PageName = model.PageName,
                    Description = model.Description,
                    Status = true
                };
                _con.AddPages.Add(pages);
                return RedirectToAction("Home");
            }
            //ModelState.AddModelError("", "Invalid");
            return View(model);
        }
    }
}
