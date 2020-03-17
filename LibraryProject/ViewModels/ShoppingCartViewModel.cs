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
    }
}