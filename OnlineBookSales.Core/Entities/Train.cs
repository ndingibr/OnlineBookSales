using OnlineBookSales.Core.Shared;
using System;

namespace OnlineBookSales.Core.Entities
{
    public class Train : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string DayofWeek { get; set; }
        public string District { get; set; }
        public string Resolution { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}
