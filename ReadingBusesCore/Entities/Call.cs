using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class Call
    {
        public string Service { get; set; }
        public string PublishedServiceName { get; set; }
        public string Destination { get; set; }
        public DateTimeOffset ScheduledArrival { get; set; }
        public DateTimeOffset ScheduledDeparture { get; set; }
    }
}
