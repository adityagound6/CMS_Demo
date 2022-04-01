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
                return View("index");
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
                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
