using readingBuses.Models;
using ReadingBusesCore;
using ReadingBusesCore.Entities;
using ReadingBusesCore.Persistence;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace readingBuses.Controllers
{
    public class HomeController : AsyncController
    {
        // yuk
        static readonly Configuration Config = new Configuration();

        public ActionResult Index()
        {
            using (var context = new Context())
            {
                var model = context.Routes.Select(p => p.Name).ToArray();
                return View(model);
            }
        }

        public async Task<ActionResult> Summary(string routeName)
        {
            using (var context = new Context())
            {
                var route = context.Routes.FirstOrDefault(r => r.Name.Equals(routeName, StringComparison.OrdinalIgnoreCase));

                // Lookup buses
                var requestedUtc = DateTime.UtcNow;

                var busInfo = new BusInfo();
                var departures = await busInfo.ListDeparturesAsync(route.TargetStops);

                var model = new TimetableSummary
                {
                    UserId = "",
                    Route = route.Name,
                    TimeStamp = requestedUtc,
                    Departures = departures.Select(ss => MapToDeparture(ss, requestedUtc)).ToArray(),
                };

                return View(model);
            }
        }


        static Departure MapToDeparture(SuggestedStop bus, DateTime nowUtc)
        {
            return new Departure
            {
                Service = bus.Service, 
                BusStop = bus.LocationName,
                DepartsIn = Utility.FriendlyTime(bus.ScheduledDeparture.UtcDateTime - nowUtc) + string.Format(" ({0:HH mm})", bus.ScheduledDeparture), 
                Destination = bus.Destination,
                Reachable = Utility.IsReachable(bus.ScheduledDeparture.UtcDateTime, nowUtc, bus.TravelTimeInMinutes, Config.DepartureMarginInSeconds).ToString()
            };
        }
      
    }
}