using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class Configuration
    {
        public Configuration()
        {
            DepartureMarginInSeconds = 60.0;
        }

        /// <summary>
        /// The required gap (plus or minus) in seconds between your expected arrival time at the stop and the departure of bus. 
        /// If the gap is +/- this value then the stop is treated as marginal.
        /// </summary>
        public double DepartureMarginInSeconds { get; set; }
    }
}
