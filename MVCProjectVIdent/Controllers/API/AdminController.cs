using MVCProjectVIdent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace MVCProjectVIdent.Controllers.API
{
    public class AdminController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var MyUser = _context.Users.Find(id);

            if (MyUser == null)
                return NotFound();
            MyUser.isDeleted = true;
            //_context.Users.Remove(MyUser);
            _context.SaveChanges();

            return Ok();
        }
    }
}
