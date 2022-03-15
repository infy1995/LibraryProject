using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Author
    {
        public int ID { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        [Display(Name = "Zdjęcie autora")]
        public string Image { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}