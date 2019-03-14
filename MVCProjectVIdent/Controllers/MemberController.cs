using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProjectVIdent.Models;



namespace MVCProjectVIdent.Controllers
{
    public class MemberController : Controller
    {
        ApplicationDbContext dbc = new ApplicationDbContext();

        // GET: Member
        public ActionResult Index()
        {
            var books = dbc.Books.Where(b => b.joinDate.Year == DateTime.Now.Year && b.joinDate.Month == DateTime.Now.Month);
            return View(books);
        }
        public ActionResult current_borrowed()
        {
            string id = Session["userId"].ToString();

            var bor = dbc.UserBook.Include("Book").Where(b => b.Memberid == id && b.status == 0 && b.isDelivered == false);
            return View(bor.ToList());
        }

        public ActionResult viewBook(int? page)
        {

            var books = dbc.Books.Where(b => b.name != null);
            return View(books.ToList());

        }




        public ActionResult details(int id)
        {
            var book = dbc.Books.FirstOrDefault(b => b.id == id);
            return PartialView(book);

        }
        //public ActionResult show_book_img(int id)
        //{
        //    //byte[] a = dbc.Books.SingleOrDefault(b => b.id == id).cover;
        //    //return File(a, "image/jpg");
        //}


        public ActionResult borrowedbooks(string date)
        {
            string id = Session["userId"].ToString();
            int year;
            int month;

            if (date != null)
            {
                string[] date_arr = date.Split('-');
                year = int.Parse(date_arr[0]);
                month = int.Parse(date_arr[1]);
            }
            else
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            var book = dbc.UserBook.Include("Book").Where(b => b.Memberid == id && b.status == MyDataType.BookStatus.isborrowking && b.startDate.Month == month && b.startDate.Year == year);
            return View(book.ToList());


        }
        public ActionResult readedbooks(string date)
        {
            string id = Session["userId"].ToString();
            int year;
            int month;

            if (date != null)
            {
                string[] date_arr = date.Split('-');
                year = int.Parse(date_arr[0]);
                month = int.Parse(date_arr[1]);
            }
            else
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            var book = dbc.UserBook.Include("Book").Where(b => b.Memberid == id && b.status == MyDataType.BookStatus.isReading && b.startDate.Month == month && b.startDate.Year == year);
            return View(book.ToList());


        }
        public ActionResult show(string id)
        {
            byte[] arr = dbc.Users.SingleOrDefault(b => b.Id == id).image;
            return File(arr, "image/jpg");
        }
    }
}