using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace emailservice.Models
{
    public class Order
    {
            public string Id { get; set; }
            public Customer Customer { get; set; }
            public string ShippingTrackingId { get; set; }
            public ShippingAddress ShippingAddress { get; set; }
            public List<Item> Items { get; set; }

        public Order()
        {
            this.Customer = new Customer();
            this.ShippingAddress = new ShippingAddress();
            this.Items = new List<Item>();
        }
    }
}
