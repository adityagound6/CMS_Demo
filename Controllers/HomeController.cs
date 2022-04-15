using CMS_Demo.Models;
using CMS_Demo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public HomeController(AppDbContext con, IConfiguration configuration)
        {
            _con = con;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index(int? id)
        {
            List<Nav_List >nav_List = new List<Nav_List>();
            nav_List = _con.AddPages.Where(x => x.Status == true).Select(x => new Nav_List{

               PageId =  x.PageId,
               PageName =  x.PageName,
               SubPageId = x.SubPageId
            }).ToList();

            var data = _con.AddPages.Find(id ?? 1);
            ViewBag.PageName = data.PageName;
            ViewBag.Description = data.Description;

            var Logo = _con.Images.Find(1).ImagePath;
            ViewBag.Logo = Logo;

            var Footer = _con.Footers.Find(1);
            if(Footer != null)
            {
                ViewBag.Footer = Footer.FooterData;
            }

            return View(nav_List);
        }
    }
}
/* var pageData = _con.AddPages.Find(id ?? 1);
            NavindexViewModel data = new NavindexViewModel();
            data.PageName = pageData.PageName;
            data.Description = pageData.Description;*/