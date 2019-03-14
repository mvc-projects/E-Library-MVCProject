using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.Models
{
    public class Member
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string id { get; set; }
        public bool isBlock { get; set; }

        public DateTime? endDate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}