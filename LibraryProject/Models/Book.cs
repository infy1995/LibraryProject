using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "ISBN")]
        [StringLength(13)]
        [RegularExpression(@"\d{13}", ErrorMessage = "ISBN musi składać sie z 13 cyfr")]
        //[Remote("IsIsbnAvailable", "Books", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string Isbn { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        [StringLength(30)]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Data Wydania")]
        [Range(1450, 3000)]
        public int ReleaseDate { get; set; }
        [Range(1, 100)]
        [Display(Name = "Ilość w bibliotece")]
        public int Quantity { get; set; }

        [Display(Name = "Zdjęcie okładki")]
        public string Image { get; set; }

        public virtual Author Author { get; set; }
        [Required]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual Publisher Publisher { get; set; }
        public Book()
        {
            this.Categories = new HashSet<Category>();
        }
    }
}