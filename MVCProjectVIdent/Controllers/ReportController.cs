using MVCProjectVIdent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProjectVIdent.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MostBorrowerdBooks()
        {
            var result = db.Books.OrderByDescending(c => c.userBooks.Count).Take(10);
            return View(result);
        }
    }
}