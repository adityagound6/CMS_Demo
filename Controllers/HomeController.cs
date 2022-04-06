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
        [HttpGet("/{id}")]
        [HttpGet("/")]
        public IActionResult Index(int? id)
        {
            List<Nav_List >nav_List = new List<Nav_List>();
            nav_List = _con.AddPages.Select(x => new Nav_List{

               PageId =  x.PageId,
               PageName =  x.PageName,
               SubPageId = x.SubPageId
            }).ToList() ;

            ViewBag.data = _con.AddPages.Find(id ?? 1).Description;
           
            return View(nav_List);
        }
    }
}
