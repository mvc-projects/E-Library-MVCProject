using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCProjectVIdent.Models;
using static MVCProjectVIdent.Models.MyDataType;
using static MVCProjectVIdent.Models.Member;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using RedirectToRouteResult = System.Web.Http.Results.RedirectToRouteResult;


namespace MVCProjectVIdent.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string fName { get; set; }
        public string lName { get; set; }

        public byte[] image { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? birthDate { get; set; }

        public System.DateTime? JoinDate { get; set; }

        [DefaultValue(false)]
        public bool isDeleted { get; set; }
        [DefaultValue(true)]
        public bool firstLogin { get; set; }
        //public blockStaues blockStaues
        //{
        //    get
        //    {
        //        if (Member.isBlock == false)
        //        {
        //            return blockStaues.notBlock;
        //        }
        //        else if (Member.isBlock && Member.endDate >= DateTime.Now)
        //        {
        //            return blockStaues.blockInWeek;
        //        }
        //        else
        //            return blockStaues.blockAfterWeek;
        //    }
        //}
        public string address { get; set; }
        public decimal? salary { get; set; }

        [InverseProperty("NotifyUser")]
        public List<Notification> NotificationsToMe { get; set; }
        [InverseProperty("User")]
        public List<Notification> NotificationsByMe { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public virtual DbSet<UserBook> UserBook { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Notification>()
        //        .HasRequired(m => m.User)
        //        .WithMany(t => t.NotificationsByMe)
        //        .HasForeignKey(m => m.UserId)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Notification>()
        //        .HasOptional(m => m.NotifyUser)
        //        .WithMany(t => t.NotificationsToMe)
        //        .HasForeignKey(m => m.ToUser)
        //        .WillCascadeOnDelete(false);
        //}

    }

}

	
public class CheckProfileAttribute : System.Web.Mvc.AuthorizeAttribute
{


	protected override bool AuthorizeCore(HttpContextBase httpContext)
	{
		var authorized = base.AuthorizeCore(httpContext);
		if (!authorized)
		{
			return false;
		}
		ApplicationDbContext db = new ApplicationDbContext();
		string authenticatedUser = httpContext.User.Identity.GetUserId();
		var user = db.Users.Find(authenticatedUser);


		if (user.firstLogin)
		{
			httpContext.Items["redirectToCompleteProfile"] = true;
			return false;
		}

		return true;
	}

	protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
	{
		if (filterContext.HttpContext.Items.Contains("redirectToCompleteProfile"))
		{
			var routeValues = new RouteValueDictionary(new
			{
				controller = "Account",
				action = "Profile",
			});
			filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(routeValues);
		}
		else
		{
			base.HandleUnauthorizedRequest(filterContext);
		}
	}
}

