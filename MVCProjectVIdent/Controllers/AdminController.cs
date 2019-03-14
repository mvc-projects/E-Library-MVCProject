using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCProjectVIdent.Models;

namespace MVCProjectVIdent.Controllers
{
    [Authorize(Roles = MyRole.BasicAdmin)]
	[CheckProfile]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> um = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        public ActionResult Index()
        {
            //return View(db.Users.ToList());
           return View(db.Users.Where(n => !n.isDeleted && n.Roles.Select(x => x.RoleId).Contains(db.Roles.FirstOrDefault(c => c.Name == MyRole.Admin).Id)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email,Password,FirstName,LastName,Salary,HireDate")] AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                //var image = Image.FromFile(Server.MapPath("~/Images/Account/Default.png"));
                var image = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/Accounts/Default.png"));
                var user = new ApplicationUser()
                {
                    Email = adminModel.Email,
                    UserName = adminModel.Email,
                    fName = adminModel.FirstName,
                    lName  = adminModel.LastName,
                    salary =  adminModel.Salary,
                    JoinDate = adminModel.HireDate,
                    firstLogin = true,
                    image = image
                };
                var result = await um.CreateAsync(user, adminModel.Password);
                if (result.Succeeded)
                {
                    await um.AddToRoleAsync(user.Id, MyRole.Admin);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("General","Somethin wrong");
                return View(adminModel);
            }

            return View(adminModel);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var MyUser = db.Users.Find(Id);

            if (MyUser == null)
                return HttpNotFound();

            var viewModel = new AdminModel
            {
                Id = MyUser.Id,
                Email = MyUser.Email,
                HireDate = (DateTime)MyUser.JoinDate,
                Salary = (decimal)MyUser.salary,
                FirstName = MyUser.fName,
                LastName = MyUser.lName,
            };


            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password,FirstName,LastName,Salary,HireDate")] AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {

                var MyUser = db.Users.Find(adminModel.Id);

                if (MyUser == null)
                    return HttpNotFound();

                MyUser.fName = adminModel.FirstName;
                MyUser.lName = adminModel.LastName;
                MyUser.JoinDate = adminModel.HireDate;
                MyUser.salary = adminModel.Salary;
                MyUser.Email = adminModel.Email;

                var notify1 =new Notification()
                {
                    CreatedAt = DateTime.Now,
                    UserId = User.Identity.GetUserId(),
                    ToUser = MyUser.Id,
                    Message = $"Your Data Updated By {User.Identity.GetUserName()} ",
                    Title = "Update Data"
                };

                var notify2 = new Notification()
                {
                    CreatedAt = DateTime.Now,
                    UserId = User.Identity.GetUserId(),
                    ToGroup = MyRole.BasicAdmin,
                    Message = $"User : {MyUser.UserName} Updated By {User.Identity.GetUserName()} ",
                    Title = "Update Data"
                };

                db.Notifications.Add(notify1);
                db.Notifications.Add(notify2);


                db.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(adminModel);

            //return RedirectToAction("adminModel");
        }

        [HttpDelete]
        public ActionResult Remove(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.isDeleted = true;
            //db.Users.Remove(user);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }  
    }
}
