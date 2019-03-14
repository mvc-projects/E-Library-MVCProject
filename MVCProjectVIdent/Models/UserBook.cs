using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MVCProjectVIdent.Models.MyDataType;

namespace MVCProjectVIdent.Models
{
    public class UserBook
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Member")]
        public string Memberid { get; set; }

        [ForeignKey("Book")]
        public int Bookid { get; set; }
        public virtual Book Book { get; set; }

        public BookStatus status { get; set; }

        public DateTime startDate { get; set; }

        public DateTime? returnDate { get; set; }

        public DateTime deliveredDate { get; set; }

        public bool isDelivered { get; set; }


        [ForeignKey("ApplicationUser")]
        public string Employeeid { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public virtual Member Member { get; set; }
    }
}