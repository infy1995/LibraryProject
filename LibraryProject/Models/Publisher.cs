using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Publisher
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Nazwa wydawcy")]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}