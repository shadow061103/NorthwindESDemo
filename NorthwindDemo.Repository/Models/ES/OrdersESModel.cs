using System;
using System.Collections.Generic;

namespace NorthwindDemo.Repository.Models.ES
{
    public class OrdersESModel
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }

        public virtual EmployeesESModel Employee { get; set; }

        public virtual ShippersESModel ShipViaNavigation { get; set; }

        public virtual ICollection<OrderDetailsESModel> OrderDetails { get; set; }

        public OrdersESModel()
        {
            OrderDetails = new HashSet<OrderDetailsESModel>();
        }
    }
}