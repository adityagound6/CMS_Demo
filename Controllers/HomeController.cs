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
            var model = _con.AddPages;
            return View(model);
        }
        [HttpPost]
        public JsonResult Getdata(int id)
        {
            var model = _con.AddPages.Find(id);
           // string data = model.Description;
            return Json(model);
        }

    }
}
