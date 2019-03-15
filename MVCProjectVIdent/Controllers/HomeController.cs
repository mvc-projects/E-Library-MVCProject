using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCProjectVIdent.Models;

namespace MVCProjectVIdent.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> um = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [Authorize]
        public ActionResult MyNotifications()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var roles = um.GetRoles(user.Id).ToList();
            var notifications = db.Notifications.Where(c => c.ToUser.Equals(user.Id) || roles.Any(x=>x.Equals(c.ToGroup))).OrderByDescending(c=>c.CreatedAt);
            return PartialView(notifications.ToList());
        }

        [HttpPost]
        public ActionResult updateNotification(int id)
        {
            var notify = db.Notifications.Find(id);
            if (notify!=null)
            {
                notify.isSeen = true;
                db.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}