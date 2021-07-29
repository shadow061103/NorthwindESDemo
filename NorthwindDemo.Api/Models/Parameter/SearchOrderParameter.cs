using System;

namespace NorthwindDemo.Api.Models.Parameter
{
    public class SearchOrderParameter
    {
        public string ShipCity { get; set; }

        public decimal? FreightMin { get; set; }

        public decimal? FreightMax { get; set; }

        public string ShipName { get; set; }

        public DateTime? StartOrderDate { get; set; }

        public DateTime? EndtOrderDate { get; set; }
    }
}