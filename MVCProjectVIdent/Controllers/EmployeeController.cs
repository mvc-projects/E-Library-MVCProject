using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProjectVIdent.Models;
using Microsoft.AspNet.Identity;

namespace MVCProjectVIdent.Controllers
{
	[Authorize(Roles = MyRole.Employee)]
	[CheckProfile]
	public class EmployeeController : Controller
	{
		ApplicationDbContext db = new ApplicationDbContext();
		// GET: Employee
		//public ActionResult Index()
		//{
		//	return View();
		//}

		public ActionResult members()
		{
			return View(db.Members.ToList());
		}
		[HttpPost]
		public ActionResult members(string prefix)
		{
			var member = (from N in db.Members.Select(a => a.ApplicationUser).ToList()
						  where N.UserName.Contains(prefix)
						  select new { N.UserName }).ToList();


			return Json(member, JsonRequestBehavior.AllowGet);
		}
		public ActionResult changeMStatus(string id)
		{
			Member member = db.Members.SingleOrDefault(a => a.id == id);
			if (member.isBlock == false)
			{
				member.isBlock = true;
			}
			else
			{
				member.isBlock = false;
			}
			db.SaveChanges();
			return RedirectToAction(nameof(members));
		}
		public ActionResult DeleteMember(string id)
		{
			List<UserBook> userBooks = db.UserBook.Where(a => a.Memberid == id && a.isDelivered == false).ToList();
			int ubcount = userBooks.Count();
			if (ubcount == 0)
			{
				List<UserBook> userBook = db.UserBook.Where(a => a.Memberid == id).ToList();
				foreach (var item in userBook)
				{
					db.UserBook.Remove(item);
				}

				db.Members.Remove(db.Members.SingleOrDefault(a => a.id == id));
				db.Users.Remove(db.Users.SingleOrDefault(a => a.Id == id));
				db.SaveChanges();
				return RedirectToAction(nameof(members));
			}
			else
			{

				ViewBag.msg = "You cannot delete this Member becouse he has books";
				return PartialView();
			}
		}

		public ActionResult todayDeliver()
		{
			List<UserBook> userBooks = db.UserBook.Where(a => a.isDelivered == false && a.returnDate.Value.Month == DateTime.Now.Month && a.returnDate.Value.Year == DateTime.Now.Year && a.returnDate.Value.Day == DateTime.Now.Day).ToList();
			return View(userBooks);
		}




		public ActionResult books()
		{
			return View(db.Books.ToList());
		}

		[HttpPost]
		public ActionResult books(string prefix)
		{

			List<string> booklist = new List<string>();
			var books = (from N in db.Books
						 select N).ToList();
			var book = (db.Books.Where(f => f.title.Contains(prefix)
											|| f.Author.fName.Contains(prefix) ||
											f.Publisher.name.Contains(prefix))).
				Select(x => new { x.title, x.Publisher.name, x.Author.fName });

			foreach (var item in book)
			{
				booklist.Add(item.fName);
				booklist.Add(item.title);
				booklist.Add(item.name);
			}


			return Json(booklist, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult Borrowbook(int id)
		{
			Book b = db.Books.SingleOrDefault(a => a.id == id);
			if (b == null)
				return HttpNotFound("book does not exist");

			ViewBag.book = b;
			ViewBag.member = new SelectList(db.Members.Where(a => a.isBlock == false).Select(a => a.ApplicationUser).ToList(), "id", "UserName");
			return View("BookView", new UserBook());
		}
		[HttpPost]
		public ActionResult Borrowbook(UserBook ub, int id, string memberid)
		{
			Book b = db.Books.SingleOrDefault(a => a.id == id);

			if (ModelState.IsValid)
			{
				List<UserBook> userBooks = db.UserBook.Where(a => a.Bookid == id && a.Memberid == ub.Memberid && a.isDelivered == false).ToList();
				int ubcount = userBooks.Count();
				if (ubcount == 0)
				{
					if (b.availableCopies > 1)
					{
						int numbook = b.availableCopies;
						numbook--;
						b.availableCopies = numbook;
						ub.Bookid = id;
						ub.Memberid = memberid;
						ub.startDate = DateTime.Now;
						ub.returnDate = DateTime.Now.AddDays(7);
						ub.status = MyDataType.BookStatus.isborrowking;
						ub.isDelivered = false;
						ub.Employeeid = User.Identity.GetUserId();
						db.UserBook.Add(ub);
						db.SaveChanges();
					}
					else
					{
						ViewBag.msg = "You have only one copy";
						return PartialView();
					}
				}
				else
				{
					ViewBag.msg = "this Member has the same book";
					return PartialView();
				}


				return RedirectToAction("BorrowedBooks");
			}


			if (b == null)
				return HttpNotFound("course does not exist");

			ViewBag.book = b;
			ViewBag.member = new SelectList(db.Members.Where(a => a.isBlock == false).ToList(), "id", "UserName");
			return View("BookView", new UserBook());
		}

		public ActionResult BorrowedBooks()
		{
			return View(db.UserBook.Where(a => a.isDelivered == false).ToList());
		}

		public ActionResult changeBStatus(int id)
		{
			UserBook userBook = db.UserBook.SingleOrDefault(a => a.id == id);
			userBook.isDelivered = true;
			userBook.deliveredDate = DateTime.Now;
			Book book = db.Books.SingleOrDefault(a => a.id == userBook.Bookid);
			int numbook = book.availableCopies;
			numbook++;
			book.availableCopies = numbook;
			db.SaveChanges();
			return RedirectToAction(nameof(BorrowedBooks));

		}

		[HttpGet]
		public ActionResult Readbook(int id)
		{
			Book b = db.Books.SingleOrDefault(a => a.id == id);
			if (b == null)
				return HttpNotFound("course does not exist");

			ViewBag.book = b;
			ViewBag.member = new SelectList(db.Members.Where(a => a.isBlock == false).Select(a => a.ApplicationUser).ToList(), "id", "UserName");
			return View(new UserBook());
		}

		[HttpPost]
		public ActionResult Readbook(UserBook ub, int id)
		{
			Book b = db.Books.SingleOrDefault(a => a.id == id);

			if (ModelState.IsValid)
			{
				List<UserBook> userBooks = db.UserBook.Where(a => a.Bookid == id && a.Memberid == ub.Memberid && a.isDelivered == false).ToList();
				int ubcount = userBooks.Count();
				if (ubcount == 0)
				{
					if (b.availableCopies > 0)
					{
						int numbook = b.availableCopies;
						numbook--;
						b.availableCopies = numbook;
						ub.Bookid = id;
						ub.startDate = DateTime.Now;
						ub.returnDate = DateTime.Now.AddHours(24);
						ub.status = MyDataType.BookStatus.isReading;
						ub.isDelivered = false;
						ub.Employeeid = User.Identity.GetUserId();
						db.UserBook.Add(ub);
						db.SaveChanges();
					}
					else
					{
						ViewBag.msg = "You donot have any copy";
						return PartialView("Borrowbook");
					}
				}
				else
				{
					ViewBag.msg = "this Member has the same book";
					return PartialView("Borrowbook");
				}


				return RedirectToAction("BorrowedBooks");
			}


			if (b == null)
				return HttpNotFound("course does not exist");

			ViewBag.book = b;
			ViewBag.member = new SelectList(db.Members.Where(a => a.isBlock == false).ToList(), "id", "UserName");
			return View("BookView", new UserBook());
		}
	}
}