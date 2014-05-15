using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class SuggestedStop
    {
        public string Service { get; set; }
        public string Destination { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public int TravelTimeInMinutes { get; set; }
        public DateTime ScheduledArrival { get; set; }
        public DateTime ScheduledDeparture { get; set; }
    }
}
