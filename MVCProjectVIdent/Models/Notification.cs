using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.Models
{
    public class Notification
    {
        [Key]
        public int id { get; set; }
        public bool isSeen { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("NotifyUser")]
        public string ToUser { get; set; }
        public string ToGroup { get; set; }

        public virtual  ApplicationUser User { get; set; }
        public virtual ApplicationUser NotifyUser { get; set; }

	    public override string ToString()
	    {
		    return $"{(User!=null?User.Email:"")}-{Title}-{Message}-{(NotifyUser!=null?NotifyUser.Email:"")}-{ToGroup}-{CreatedAt.ToString()}";
	    }
    }
}