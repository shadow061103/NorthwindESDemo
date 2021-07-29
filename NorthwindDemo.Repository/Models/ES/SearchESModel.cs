using System;

namespace NorthwindDemo.Repository.Models.ES
{
    public class SearchESModel
    {
        public string ShipCity { get; set; }

        public decimal? FreightMin { get; set; }

        public decimal? FreightMax { get; set; }

        public string ShipName { get; set; }

        public DateTime? StartOrderDate { get; set; }

        public DateTime? EndtOrderDate { get; set; }
    }
}