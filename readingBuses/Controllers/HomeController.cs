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
                var now = DateTime.Now;

                var busInfo = new BusInfo();
                var departures = await busInfo.ListDeparturesAsync(route.TargetStops);

                var model = new TimetableSummary
                {
                    UserId = "dan",
                    Route = route.Name,
                    TimeStamp = now,
                    Departures = departures.Select(ss => MapToDeparture(ss, now)).ToArray(),
                };

                return View(model);
            }
        }


        static Departure MapToDeparture(SuggestedStop bus, DateTime now)
        {
            return new Departure
            {
                Service = bus.Service, 
                BusStop = bus.LocationName, 
                DepartsIn = Utility.FriendlyTime(bus.ScheduledDeparture - now), 
                Destination = bus.Destination,
                Reachable = Utility.IsReachable(bus.ScheduledDeparture, now, bus.TravelTimeInMinutes, Config.DepartureMarginInSeconds).ToString()
            };
        }
      
    }
}