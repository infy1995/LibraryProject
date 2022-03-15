using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string Login { get; set; }
        [Phone]
        [Display(Name = "Numer Telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        public DateTime? BirthDate { get; set; }
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [StringLength(30)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [StringLength(30)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [StringLength(30)]
        [Display(Name = "Numer domu")]
        public string HouseNumber { get; set; }
        [StringLength(30)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [StringLength(30)]
        [Display(Name = "Miejscowość")]
        public string Town { get; set; }
        public string geoLocationLat { get; set; }
        public string geoLocationLng { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public virtual Order HelperOrders { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string FullAdress 
        {
            get { return Street + " " + HouseNumber + " " + PostalCode + " " + Town; }
        }
    }
}