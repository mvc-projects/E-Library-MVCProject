using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.Models
{
    [Serializable]
    public class Book
    {
        [Key]
        public int id { get; set; }
		[Range(0,5000,ErrorMessage = "Copies Count Must Be Between 0 and 5000")]
        public int copiesCount { get; set; }
	    [Range(0, 5000, ErrorMessage = "Available Count Must Be Between 0 and 5000")]
		public int availableCopies { get; set; }

        [Required]
        public string title { get; set; }

        [ForeignKey("Author")]
        public int AutherId { get; set; }
        public virtual Author Author { get; set; }


        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }


        public string cover { get; set; }

        [Required]
        public string name { get; set; }

        public string source { get; set; }
        public bool isDeleted { get; set; }

        public DateTime joinDate { get; set; }
        public DateTime publishDate { get; set; }

        public virtual List<UserBook> userBooks { get; set; }
    }
}