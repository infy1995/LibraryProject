using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryProject.Models;

namespace LibraryProject.ViewModels
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
        public int RentedCount { get; set; }
        public int ReturnedCount { get; set; }
        public IEnumerable<Order> Placed { get; set; }
        public IEnumerable<OrderDetail> Rented { get; set; }
        public IEnumerable<Order> Returned { get; set; }
    }
}