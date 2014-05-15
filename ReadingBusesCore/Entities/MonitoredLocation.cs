using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class MonitoredLocation
    {
        public MonitoredLocation()
        {
            Calls = new Call[] { };
        }

        public string Timestamp { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Naptan { get; set; }
        
        public Call[] Calls { get; set; }
    }
}
