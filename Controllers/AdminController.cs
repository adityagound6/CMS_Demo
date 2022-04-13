using CMS_Demo.Models;
using CMS_Demo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _con;

        public AdminController(AppDbContext con)
        {
            _con = con;
           
        }

      private bool Redirect()
        {
            string user = HttpContext.Session.GetString("UserName");
            if(user == null)
            {
                 RedirectToAction("Login");
            }
            return false;
        }

        [HttpGet]
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
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            try
            {
                var user = _con.Users.Where(s => s.Email == Email & s.Password == Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "InValid UserId or Password.");
                    return View();
                }
                HttpContext.Session.SetString("UserName", user.UserName);
                ViewBag.Session = HttpContext.Session.GetString("UserName");

                HttpContext.Session.SetInt32("Permission", user.Permission);
                ViewBag.Permission = HttpContext.Session.GetInt32("Permission");

                if (user.Permission != 1)
                {
                    var menuPermission = _con.UserRoles.Where(x => x.UserId == user.UserId);
                    for (var i = 1; i <= menuPermission.Count(); i++)
                    {
                        HttpContext.Session.SetInt32("Permission" + i, user.Permission);
                    }
              }
                
               /* var userRole = from UR in _con.UserRoles.Where(x => x.UserId == user.UserId)
                               join AR in _con.AddRoles
                               on UR.RoleId equals AR.RoleId
                               select new AddRole
                               { RoleName = AR.RoleName };*/

                return RedirectToAction("index");
            }
            catch(DbException)
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

        [AllowAnonymous]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        #endregion

        #region `Side MenuBar Items: AddPage, Add Subpage, Manage Page,  Add User, Manage SubUser, Delete User, Edit SubUser`

      
        [HttpGet]
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
                    Status = true,
                    //SubPageId = null
                   
                };
                _con.AddPages.Add(pages);
                var status = _con.SaveChanges();
                if(status == 1)
                {
                    return RedirectToAction("AddPage");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddSubPage()
        {
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
                    SubPageId = model.PageId

                };
                _con.AddPages.Add(pages);
                var status = _con.SaveChanges();
                if (status == 1)
                {
                    return RedirectToAction("AddSubPage");
                }
            }

            return View(model);
        }

        [Route("Admin/ManagePage")]
        public IActionResult ManagePage()
        {
             ViewBag.PageList = PageList();
            //ViewBag.userRoles = RolesData();
            return View();
        }

        [Route("Admin/ManagePage/{id}")]
        public IActionResult ManagePage(int id)
        {
            AddPage data = _con.AddPages.Find(id);
            if (data != null)
            {
                ManagePageViewModel managePageView = new ManagePageViewModel
                {
                    PageId = data.PageId,
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
        public IActionResult ManagePage(AddPage model)
        {
            AddPage data = _con.AddPages.Find(model.PageId);
            data.PageName = model.PageName;
            data.Description = model.Description;
            data.Status = model.Status;
            _con.AddPages.Update(data);
            _con.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Managepage");
            
        }


        [HttpGet]
        public IActionResult AddSubUser()
        {
           // ViewBag.userRoles = RolesData();
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

        public IActionResult ManageSubUser()
        {
            var Users = _con.Users;

            return View(Users);
        }

        public bool DeleteSubUser(int id)
        {
            var User = _con.Users.Find(id);
            if(User != null)
            {
                _con.Users.Remove(User);
               int status = _con.SaveChanges();
                if(status == 1)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public IActionResult EditSubUser(int id)
        {
            Users User = _con.Users.Find(id);
            /*ist<UserRole> role =  _con.UserRoles.Where(x => x.UserId == id).Select(new UserRole { 
                RoleId,
               
            });*/
            ViewBag.Roles = _con.AddRoles;
            EditSubUserViewModel subUserViewModel = new EditSubUserViewModel
            {
                UserId = User.UserId,
                Email = User.Email,
                Password = User.Password,
                //RoleId = role.RoleId,
                
                
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
            if(User == null)
            {
                return View(model);
            }
            
            User.Email = model.Email;
            User.Password = model.Password;
            var checkuser = _con.UserRoles.Where(x => x.UserId == model.UserId).ToList();
            foreach(var userRol in checkuser)
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
            if (count == 5)
            {
                User.Permission = 1;
            }
            else
            {
                User.Permission = 0;
            }
            _con.Users.Update(User);
            _con.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region "Get data of Pages"
        [HttpPost]
        public JsonResult GetData(int id)
        {
            var data = _con.AddPages.Find(id);
            return Json(data);
        }
        public IList<AddPage> PageList()
        {
            List<AddPage> PageList = new List<AddPage>();
            PageList = _con.AddPages.ToList();
            return PageList;
        }

        #endregion

        #region "Remote Validation for Email and Page"

        [AcceptVerbs("Get","Post")]
        public JsonResult IsEmailInUsed(string email)
        {
            var user = _con.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user == null) {
                return Json(true);
             }
            else {
                return Json("This Email Is already in Used.");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsPageInUsed(string pagename)
        {
            var page = _con.AddPages.Where(x => x.PageName == pagename).FirstOrDefault();
            if (page == null)
            {
                return Json(true);
            }
            else
            {
                return Json(pagename + " Page Is already Exist.");
            }
        }

        #endregion
    }
}
