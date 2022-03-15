using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static LibraryProject.Models.Order;

namespace LibraryProject.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public string CartID { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public int BookID { get; set; }
        public OrderTypeEnum orderType { get; set; }
        public virtual Book Book { get; set; }
    }
}