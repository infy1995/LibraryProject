using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LibraryProject.Models;

namespace LibraryProject.ViewModels
{
    public class CreateBookViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        [StringLength(13)]
        [RegularExpression(@"\d{13}", ErrorMessage = "ISBN musi składać sie z 13 cyfr")]
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
        public Author Author { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Publisher Publisher { get; set; }

        public int SelectedAuthorID { get; set; }
        public int SelectedPublisherID { get; set; }
        public List<int> SelectedCategoryID { get; set; }

        public IEnumerable<Author> AuthorsCollection { get; set; }
        public IEnumerable<Publisher> PublishersCollection { get; set; }
        public IEnumerable<Category> CategoriesCollection { get; set; }

        public Book GetBook()
        {
            return new Book
            {
                ID = ID,
                Isbn = Isbn,
                Title = Title,
                Description = Description,
                ReleaseDate = ReleaseDate,
                Quantity = Quantity,
                Image = Image,
                Author = Author,
                Categories = Categories,
                Publisher = Publisher
            };
        }
    }
}