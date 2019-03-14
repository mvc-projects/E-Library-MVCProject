using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using MVCProjectVIdent.Models;

namespace MVCProjectVIdent.Controllers
{
	[Authorize(Roles = MyRole.BasicAdmin_Admin)]
	[CheckProfile]
	public class BookController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
      
        // GET: Book
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(c=>!c.isDeleted);
            return View(books.ToList());
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            ViewBag.AutherId = new SelectList(db.Authors, "id", "email");
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "name");
            ViewBag.PublisherId = new SelectList(db.Publishers, "id", "name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "copiesCount,availableCopies,title,AutherId,PublisherId,CategoryId,cover,name,publishDate")] Book book, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var extn = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                    string bookfile = $"{book.name}-{DateTime.Now.Ticks}{extn}";
                    image.SaveAs(Server.MapPath($"~/images/books/{bookfile}"));
                    book.cover = bookfile;
                }
                book.joinDate= DateTime.Now;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AutherId = new SelectList(db.Authors, "id", "email", book.AutherId);
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "name", book.CategoryId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "id", "name", book.PublisherId);
            return View(book);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AutherId = new SelectList(db.Authors, "id", "email", book.AutherId);
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "name", book.CategoryId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "id", "name", book.PublisherId);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,copiesCount,availableCopies,title,AutherId,PublisherId,CategoryId,cover,name,publishDate")] Book book, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var extn = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                    string bookfile = $"{book.name}-{DateTime.Now.Ticks}{extn}";
                    image.SaveAs(Server.MapPath($"~/images/books/{bookfile}"));
                    book.cover = bookfile;
                }

                var newBook = db.Books.Find(book.id);
                newBook.copiesCount = book.copiesCount;
                newBook.title = book.title;
                newBook.AutherId = book.AutherId;
                newBook.PublisherId = book.PublisherId;
                newBook.CategoryId = book.CategoryId;
                newBook.cover = book.cover!=null?book.cover:newBook.cover;
                newBook.name = book.name;
                newBook.publishDate = book.publishDate;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AutherId = new SelectList(db.Authors, "id", "email", book.AutherId);
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "name", book.CategoryId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "id", "name", book.PublisherId);
            return View(book);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            book.isDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
