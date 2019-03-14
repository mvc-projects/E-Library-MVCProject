using Hangfire;
using Microsoft.Owin;
using MVCProjectVIdent.Models;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(MVCProjectVIdent.Startup))]
namespace MVCProjectVIdent
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate(
                () => block(),
                Cron.Daily);
            RecurringJob.AddOrUpdate(
                () => unblock(),
                Cron.Daily);
            app.UseHangfireServer();
        }
        public void block()
        {
            List<UserBook> userBooks = db.UserBook.Where(a => a.isDelivered == false && a.returnDate.Value.Month == DateTime.Now.Month && a.returnDate.Value.Year == DateTime.Now.Year && a.returnDate.Value.Day == DateTime.Now.Day).ToList();
            if (userBooks.Count != 0)
            {
                foreach (var item in userBooks)
                {
                    Member member = db.Members.SingleOrDefault(a => a.id == item.Memberid);
                    if (member.endDate == null)
                    {
                        member.isBlock = true;
                        member.endDate = DateTime.Now.AddDays(7);
                    }
                    db.SaveChanges();
                }
            }
        }
        public void unblock()
        {
            List<Member> members = db.Members.Where(a => a.isBlock == true && a.endDate.Value.Month == DateTime.Now.Month && a.endDate.Value.Year == DateTime.Now.Year && a.endDate.Value.Day == DateTime.Now.Day).ToList();
            if (members.Count != 0)
            {
                foreach (var item in members)
                {
                    item.isBlock = false;
                    item.endDate = null;
                    db.SaveChanges();
                }
            }
        }
    }
}
