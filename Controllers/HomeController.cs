using CMS_Demo.Models;
using CMS_Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public object UserListViewModel { get; private set; }

        public IActionResult Index()
        {
            var model = _con.AddPages.ToList();
            return View(model);
        }
        public JsonResult Getdata(int id)
        {
            var data = _con.AddPages.Find(id);
            string P = data.Description;
            return  Json(P);
        }
    }
}
