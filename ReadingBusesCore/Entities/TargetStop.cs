using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class TargetStop
    {
        /// <summary>
        /// Friendly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Time taken to travel to this location
        /// </summary>
        public int MinutesToLocation { get; set; }

        /// <summary>
        /// ID of location e.g. 039028140006
        /// </summary>
        public string LocationId { get; set; }

        public GeoCoordinate Location { get; set; }

        /// <summary>
        /// Services we can take from this location
        /// </summary>
        public string[] Services { get; set; }
    }
}
