using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Category()
        {
            this.Books = new HashSet<Book>();
        }
    }
}