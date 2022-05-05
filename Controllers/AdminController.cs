using CMS_Demo.Models;
using CMS_Demo.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace CMS_Demo.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _Environment;

        public AdminController(AppDbContext con, IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment)
        {
            _con = con;
            _configuration = configuration;
            _Environment = webHostEnvironment;
        }
        [Route("admin/")]
        public IActionResult Index()
        {
            return View();
        }

        #region "Login,register and logout"
        [AllowAnonymous]
        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            try
            {
                var user = _con.Users.Where(s => s.Email == model.Email & s.Password == model.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "InValid UserId or Password.");
                    return View();
                }
                
                var userRoles = _con.UserRoles.Where(x => x.UserId == user.UserId).Select(y => y.RoleId).ToList();
                for (var i = 0; i < userRoles.Count(); i++)
                {
                    var userRolesId = userRoles[i];
                    HttpContext.Session.SetInt32($"Permission{userRolesId}", userRolesId);
                }
                var claims = new List<Claim>
                                 {
                                     new Claim("UserId",user.UserId.ToString()),
                                     new Claim("UserName",user.UserName),
                                     new Claim(ClaimTypes.Name,user.Name),
                                     new Claim(ClaimTypes.Email, user.Email),
                                    // new Claim(ClaimTypes.myNewClaim,user.Email),
                                     //new Claim("",userRoles),
                                 };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    IsPersistent = true,

                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),

                };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("index","admin");
            }
            catch (DbException)
            {
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
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
                    Password = model.Password,
                    Permission = 0
                };
                _con.Users.Add(user);
                var status = _con.SaveChanges();
                if (status == 1)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    ViewBag.Session = HttpContext.Session.GetString("UserName");
                    return RedirectToAction("index");
                }
                return View();
            }
            return View();
        }
        [AllowAnonymous]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        #endregion

        #region "Admin Panel Methods"
        [HttpGet]
        public IActionResult ManageStaticSettings()
        {
            if (HttpContext.Session.GetInt32("Permission6") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            var footer = _con.Footers.Find(1).FooterData;
            var logo = _con.Images.Find(1).ImagePath;
            ManageStatics manageStatics = new ManageStatics
            {
                Footer = footer,

            };

            return View(manageStatics);
        }

        [HttpPost]
        public IActionResult ManageStaticSettings(ManageStatics model)
        {
            if(model.SiteLogo != null)
            {
                string uniqueFileName = ProcesFileUpload(model);
                int OldLogoCount = _con.Images.Count();
                if(OldLogoCount != 0)
                {
                    ImagesPaths OldLogo = _con.Images.Find(OldLogoCount);
                    if (OldLogo != null)
                    {
                        OldLogo.ImagePath = uniqueFileName;
                        _con.Images.Update(OldLogo);
                    }
                }
                else
                {
                    ImagesPaths images  = new ImagesPaths
                    {
                        ImagePath = uniqueFileName
                    };
                    _con.Images.Add(images);
                }

            }
            if(model.Footer != null)
            {
                int FooterCount = _con.Footers.Count();
                Footer oldFooter = _con.Footers.Find(FooterCount);
                if(oldFooter != null)
                {
                    oldFooter.FooterData = model.Footer;
                    _con.Footers.Update(oldFooter);
                }
                else
                {
                    Footer footer = new Footer
                    {
                        FooterData = model.Footer
                    };
                    _con.Footers.Add(footer);
                }
            }
            _con.SaveChanges();
            return RedirectToAction("ManageStaticSettings");
        }

        private string ProcesFileUpload(ManageStatics model)
        {
            string uniqueFileName = null;
            if (model.SiteLogo != null)
            {
                // var uploadFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("Images")));
                string uploadsFolder = Path.Combine(_Environment.WebRootPath, "Images");
                //string uploadsFolder = Path.Combine("./wwwroot/","Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.SiteLogo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.SiteLogo.CopyTo(filestream);
                }
            }
            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddPage()
        {
            if (HttpContext.Session.GetInt32("Permission1") == null)
            {
                return RedirectToAction("AccessDenied");
            }
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
                    Status = true,
                    //SubPageId = null

                };
                _con.AddPages.Add(pages);
                var status = _con.SaveChanges();
                if (status == 1)
                {
                    return RedirectToAction("AddPage");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddSubPage()
        {
            if (HttpContext.Session.GetInt32("Permission2") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            ViewBag.PageList = PageList();
            //ViewBag.userRoles = RolesData();
            return View();
        }
        [HttpPost]
        public IActionResult AddSubPage(AddSubPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                    AddPage pages = new AddPage
                    {
                        PageName = model.PageName,
                        Description = model.Description,
                        Status = true,
                        SubPageId = model.PageIds
                    };
                    _con.AddPages.Add(pages);
                }
                var status = _con.SaveChanges();
                if (status == 1)
                {
                    return RedirectToAction("AddSubPage");
                }
            return View(model);
        }

        public IList<AddPage> PageList()
        {
            List<AddPage> PageList = new List<AddPage>();
            PageList = _con.AddPages.ToList();
            return PageList;
        }


        [Route("Admin/ManagePage")]
        public IActionResult ManagePage()
        {
            if (HttpContext.Session.GetInt32("Permission3") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            ViewBag.PageList = PageList();
            //ViewBag.userRoles = RolesData();
            return View("ManagePage");
        }

        [Route("Admin/ManagePage/{id}")]
        public IActionResult ManagePage(int id)
        {
            if (HttpContext.Session.GetInt32("Permission3") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            AddPage data = _con.AddPages.Find(id);
            if (data != null)
            {
                ManagePageViewModel managePageView = new ManagePageViewModel
                {
                    PageId = data.PageId,
                    NewPageName = data.PageName,
                    PageName = data.PageName,
                    Description = data.Description,
                    Status = data.Status
                };
                ViewBag.PageList = PageList();
                return View(managePageView);
            }
            return View();
        }

        [HttpPost("Admin/ManagePage/{id}")]
        public IActionResult ManagePage(ManagePageViewModel model)
        {
            AddPage data = _con.AddPages.Find(model.PageId);
            data.PageName = model.NewPageName;
            data.Description = model.Description;
            data.Status = model.Status;
            _con.AddPages.Update(data);
            _con.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Managepage");

        }

        [HttpPost]
        public JsonResult GetData(int id)
        {
            var data = _con.AddPages.Find(id);
            return Json(data);
        }

        [HttpGet]
        public IActionResult AddSubUser()
        {
            if (HttpContext.Session.GetInt32("Permission4") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            return View();
        }
        [HttpPost]

        public IActionResult AddSubUser(AddSubUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Email,
                    Password = model.Password,
                    Permission = 0
                };
                _con.Users.Add(user);
                var status = _con.SaveChanges();
                if (status == 1)
                {
                    return RedirectToAction("AddSubUser");
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult ManageSubUser()
        {
            if (HttpContext.Session.GetInt32("Permission5") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            var Users = _con.Users;

            return View(Users);
        }

        public bool DeleteSubUser(int id)
        {

            var User = _con.Users.Find(id);
            if (User != null)
            {
                _con.Users.Remove(User);
                int status = _con.SaveChanges();
                if (status == 1)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public IActionResult EditSubUser(int id)
        {
            if (HttpContext.Session.GetString("Permission5") == null)
            {
                return RedirectToAction("AccessDenied");
            }
            Users User = _con.Users.Find(id);
            EditSubUserViewModel subUserViewModel = new EditSubUserViewModel
            {
                UserId = User.UserId,
                Email = User.Email,
                Password = User.Password
            };
            var Addroleuser = _con.AddRoles.ToList();
            for (int j = 0; j < Addroleuser.Count; j++)
            {
                subUserViewModel.AddRole.Add(Addroleuser[j].RoleId, Addroleuser[j].RoleName);
            }
            var role = _con.UserRoles.Where(x => x.UserId == id).ToList();
            if (role.Count != 0)
            {
                for (int i = 0; i < role.Count; i++)
                {
                    subUserViewModel.isActive.Add(role[i].RoleId, true);
                }
                var roleNot = _con.AddRoles.ToList();
                for (int j = 0; j < roleNot.Count; j++)
                {
                    if (subUserViewModel.isActive.ContainsKey(roleNot[j].RoleId))
                    {
                        continue;
                    }
                    else
                    {
                        subUserViewModel.isActive.Add(roleNot[j].RoleId, false);
                    }
                }
            }
            else
            {
                var roleNot = _con.AddRoles.ToList();
                for (int j = 0; j < roleNot.Count; j++)
                {
                    subUserViewModel.isActive.Add(roleNot[j].RoleId, false);
                }
            }
            return View(subUserViewModel);
        }
        [HttpPost]
        public IActionResult EditSubUser(EditSubUserViewModel model)
        {
            Users User = _con.Users.Find(model.UserId);
            if (User == null)
            {
                return View(model);
            }

            User.Email = model.Email;
            User.Password = model.Password;
            var checkuser = _con.UserRoles.Where(x => x.UserId == model.UserId).ToList();
            foreach (var userRol in checkuser)
            {
                var us = _con.UserRoles.Find(userRol.Id);
                _con.UserRoles.Remove(us);
                _con.SaveChanges();
            }
            var count = 0;
            foreach (var x in model.isActive)
            {
                UserRole role = new UserRole();

                if (x.Value == true)
                {
                    count++;
                    role.UserId = model.UserId;
                    role.RoleId = x.Key;
                    _con.UserRoles.Add(role);
                    _con.SaveChanges();

                }
            }
            if (count == 6)
            {
                User.Permission = 1;
            }
            else
            {
                User.Permission = 0;
            }
            _con.Users.Update(User);
            _con.SaveChanges();
            return RedirectToAction("EditSubUser");
        }
        #endregion

        #region "Remote Validation"
        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsEmailInUsed(string email)
        {
            var user = _con.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json("This Email Is already in Used.");
            }
        }
        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsPageInUsed(string pagename, string NewPageName)
      {
            if(pagename != NewPageName)
            {
                var page = _con.AddPages.Where(x => x.PageName == (NewPageName ?? pagename)).FirstOrDefault();
                if (page == null)
                {
                    return Json(true);
                }
                else
                {
                    return Json((NewPageName ?? pagename) + " Page Is already Exist.");
                }
            }
            else
            {
                return Json(true);
            }
            
        }
        #endregion
    }
}
