using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Profile HelperProfile { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public OrderTypeEnum OrderType { get; set; }

        public enum OrderTypeEnum { 
            Pickup,
            Delivery
        };

        public enum OrderStatusEnum
        {
            Placed,
            Ready,
            Rented,
            Returned
        };
    }
}