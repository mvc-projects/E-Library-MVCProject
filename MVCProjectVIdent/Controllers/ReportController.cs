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


	    public ActionResult MostBorrowerdUsers()
	    {
		    var result = db.Members.OrderByDescending(c => c.UserBooks.Count).Take(10);
		    return View(result.ToList());
	    }

	    public ActionResult LessBorrowerdBooks()
	    {
		    var result = db.Books.OrderBy(c => c.userBooks.Count).Take(10);
		    return View(result.ToList());
	    }


	    public ActionResult LessBorrowerdUsers()
	    {
		    var result = db.Members.OrderBy(c => c.UserBooks.Count).Take(10);
		    return View(result.ToList());
	    }

	    public ActionResult CurrentMembers()
	    {
		    var result = db.Members.Where(c=>!c.ApplicationUser.isDeleted);
		    return View(result.ToList());
	    }


	    public ActionResult CurrentUsers()
	    {
		    var members = db.Members.ToList();

			var result = db.Users.Where(c=>!c.isDeleted&&members.Exists(x=>x.id!=c.Id));
		    return View(result.ToList());
	    }

	    public ActionResult currentBorrowedBooks()
	    {
		    var result = db.Books.Where(c => c.userBooks.Exists(x =>x.status == MyDataType.BookStatus.isborrowking &&!x.isDelivered));
		    return View(result.ToList());
	    }


	    public ActionResult currentReadBooks()
	    {
		    var result = db.Books.Where(c => c.userBooks.Exists(x => x.status == MyDataType.BookStatus.isReading && !x.isDelivered));
		    return View(result.ToList());
	    }

	    public ActionResult LateBooks()
	    {
		    var result = db.Books.Where(c => c.userBooks.Exists(x => !x.isDelivered && x.returnDate>DateTime.Now));
		    return View(result.ToList());
	    }
	    public ActionResult LateMembers()
	    {
		    var result = db.Members.Where(c => c.UserBooks.Exists(x => !x.isDelivered && x.returnDate > DateTime.Now));
		    return View(result.ToList());
	    }





	}
}