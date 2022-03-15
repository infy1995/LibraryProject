using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryProject.Models;

namespace LibraryProject.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public Profile Profile { get; set; }
        public List<Order> Orders { get; set; }
        public string JsonOrders{ get; set; }
    }
}