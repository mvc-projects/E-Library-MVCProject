using MVCProjectVIdent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCProjectVIdent.ViewModels;

namespace MVCProjectVIdent.Controllers
{
    public class BasicAdminController : Controller
    {
        private ApplicationDbContext _context;

        public BasicAdminController()
        {
            _context = new ApplicationDbContext();
        }
        //static List<ApplicationUser> BasicAdmins = new List<ApplicationUser>()
        //new ApplicationUser {
        //};

        // GET: BasicAdmin
        public ActionResult Index()
        {
            var usersWithRoles = (from user in _context.Users
                select new
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    //Email = user.Email,
                    RoleNames = (from userRole in user.Roles
                        join role in _context.Roles on userRole.RoleId
                            equals role.Id
                        select role.Name).ToList()
                }).ToList().Select(p => new UsersWithRuleViewModel()

            {
                UserId = p.UserId,
                Username = p.Username,
                //Email = p.Email,
                Role = string.Join(",", p.RoleNames)
            });


            return View(usersWithRoles);
        }

        // GET: CreateBasicAdmin

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersWithRuleViewModel model)
        {


            var admin = new ApplicationUser()
            {
                UserName = model.Username,
                JoinDate = model.JoinDate,
                salary = model.salary,
            };

            _context.Users.Add(admin);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}