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
        [HttpGet("home/index")]
        [HttpGet("/")]
        public IActionResult Index()
        {
            AddPage data = _con.AddPages.Find(1);
            List<AddPage> model = _con.AddPages.ToList();
            IndexViewModel mod = new IndexViewModel();
            if (mod.NavBar == null)
            {
                mod.NavBar = new List<AddPage>();
            }
            foreach (var addPage in model)
            {
                mod.NavBar.Add(addPage);
            }
            
            if (mod.addPages == null)
            {
                mod.addPages = new List<AddPage>();
            }
            mod.addPages.Add(data);
            return View(mod);
        }
        //[HttpGet("/")]
        [HttpGet("home/index/{id?}")]
        // [HttpGet]
        public IActionResult Index(int id)
        {
            AddPage data = _con.AddPages.Find(id);
            List<AddPage> addPages = _con.AddPages.ToList();
            IndexViewModel model = new IndexViewModel();
            if (model.NavBar == null)
            {
                model.NavBar = new List<AddPage>();
            }
            foreach (var addPage in addPages)
            {
                model.NavBar.Add(addPage);
            }
            if (model.addPages == null)
            {
                model.addPages = new List<AddPage>();
            }
            model.addPages.Add(data);
           // string data = model.Description;
            return View("index",model);
        }

    }
}
