using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualBasic.ApplicationServices;
using MVCProjectVIdent.Models;

namespace MVCProjectVIdent.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<MVCProjectVIdent.Models.ApplicationDbContext>
	{
		UserManager<ApplicationUser> um =
			new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(MVCProjectVIdent.Models.ApplicationDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.


			if (!context.Roles.Any(r => r.Name == MyRole.BasicAdmin))
			{
				context.Roles.Add(new IdentityRole(MyRole.BasicAdmin));
			}

			if (!context.Roles.Any(r => r.Name == MyRole.Admin))
			{
				context.Roles.Add(new IdentityRole(MyRole.Admin));

			}

			if (!context.Roles.Any(r => r.Name == MyRole.Employee))
			{
				context.Roles.Add(new IdentityRole(MyRole.Employee));

			}

			if (!context.Roles.Any(r => r.Name == MyRole.Member))
			{
				context.Roles.Add(new IdentityRole(MyRole.Member));

			}

			if (!context.Users.Any(u => u.UserName == "m@m.com"))
			{
				var store = new UserStore<ApplicationUser>(context);
				var manager = new UserManager<ApplicationUser>(store);
				var user = new ApplicationUser()
				{
					Email = "m@m.com",
					UserName = "m@m.com",
					JoinDate = DateTime.Now,
					firstLogin = true,
				};
				manager.Create(user, "123456");
				manager.AddToRole(user.Id, MyRole.BasicAdmin);
			}
		
		}
	}
}
