using LibraryProject.Models;
using LibraryProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.ViewModels
{
    public class CheckoutSummaryViewModel
    {
        public ShoppingCart shoppingCart { get; set; }
        public Order helperOrder { get; set; }
    }
}