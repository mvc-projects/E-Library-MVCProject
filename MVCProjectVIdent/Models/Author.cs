using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.Models
{
    [Serializable]
    public class Author
    {
        public int id { get; set; }

        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string fName { get; set; }

        [Required]
        public string lName { get; set; }

        public string image { get; set; }


        public DateTime? birthDate { get; set; }
        public bool isDeleted { get; set; }
    }
}