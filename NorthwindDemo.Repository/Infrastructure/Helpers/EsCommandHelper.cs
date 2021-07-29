using Nest;
using NorthwindDemo.Repository.Models.ES;
using System;
using System.Collections.Generic;

namespace NorthwindDemo.Repository.Infrastructure.Helpers
{
    public class EsCommandHelper
    {
        public static TermQuery GetShipCountyContainer(string county)
        {
            if (string.IsNullOrEmpty(county))
            {
                return null;
            }

            return new TermQuery
            {
                Field = Infer.Field<OrdersESModel>(c => c.ShipCountry.Suffix("keyword")),
                Value = county
            };
        }

        public static NumericRangeQuery GetFreightMinContainer(decimal? freightMin)
        {
            if (freightMin.HasValue.Equals(false))
            {
                return null;
            }

            return new NumericRangeQuery
            {
                Field = Infer.Field<OrdersESModel>(c => c.Freight),
                GreaterThanOrEqualTo = (double)freightMin.Value
            };
        }

        public static NumericRangeQuery GetFreightMaxContainer(decimal? freightMax)
        {
            if (freightMax.HasValue.Equals(false))
            {
                return null;
            }

            return new NumericRangeQuery
            {
                Field = Infer.Field<OrdersESModel>(c => c.Freight),
                LessThanOrEqualTo = (double)freightMax.Value
            };
        }

        public static BoolQuery GetShipNameContainer(string shipName)
        {
            if (string.IsNullOrWhiteSpace(shipName))
            {
                return null;
            }

            return new BoolQuery()
            {
                Should = new List<QueryContainer>()
                {
                    new MatchPhraseQuery()
                    {
                        Field = Infer.Field<OrdersESModel>(p => p.ShipName),
                        Query = shipName
                    }
                }
            };
        }

        public static DateRangeQuery GetOrderDateContainer(DateTime? startDate, DateTime? endDate)
        {
            if (startDate is null || endDate is null)
            {
                return null;
            }

            return new DateRangeQuery
            {
                Field = Infer.Field<OrdersESModel>(p => p.OrderDate),
                GreaterThanOrEqualTo = startDate,
                LessThanOrEqualTo = endDate
            };
        }
    }
}