using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Services.Description;

namespace LibraryProject.ViewModels
{
    public class ShoppingCartAddViewModel
    {
        public string MessageSuccess { get; set; }
        public string MessageWarning { get; set; }
        public string MessageDanger { get; set; }
    }
}