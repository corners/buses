using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace readingBuses.Models
{
    public class Departure
    {
        public string Service { get; set; }
        public string BusStop { get; set; }
        public string DepartsIn { get; set; }
        public DateTime DepartsUtc { get; set; }
        public string Destination { get; set; }
        public string Reachable { get; set; }
    }
}