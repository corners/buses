using ReadingBusesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace readingBuses.Models
{
    public class TimetableSummary
    {
        public string UserId { get; set; }
        public string Route { get; set; }
        public DateTime TimeStamp { get; set; }
        public Departure[] Departures { get; set; }
    }
}