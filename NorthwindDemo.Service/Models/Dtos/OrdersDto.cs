using System;
using System.Collections.Generic;

namespace NorthwindDemo.Service.Models.Dtos
{
    public class OrdersDto
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

        public virtual EmployeesDto Employee { get; set; }

        public virtual ShippersDto ShipViaNavigation { get; set; }

        public virtual ICollection<OrderDetailsDto> OrderDetails { get; set; }

        public OrdersDto()
        {
            OrderDetails = new HashSet<OrderDetailsDto>();
        }
    }
}