using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public static class Map
    {
        public static SuggestedStop ToSuggestedStop(TargetStop targetStop, CallsAtStopResponse callAtStop, Call call)
        {
            return new SuggestedStop
            {
                Service = call.Service,
                Destination = call.Destination,
                LocationId = callAtStop.MonitoredLocation.Id,
                LocationName = callAtStop.MonitoredLocation.Name,
                TravelTimeInMinutes = targetStop.MinutesToLocation,
                ScheduledArrival = call.ScheduledArrival,
                ScheduledDeparture = call.ScheduledDeparture,
            };
        }
    }
}
