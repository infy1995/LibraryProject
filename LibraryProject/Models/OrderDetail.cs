using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public DetailStatusEnum DetailStatus { get; set; }
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }

        public enum DetailStatusEnum
        {
            Placed,
            Ready,
            Rented,
            Returned
        };
    }

}