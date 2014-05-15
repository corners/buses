using readingBuses.Models;
using ReadingBusesCore;
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

        // ugh get rid
        static Dictionary<NamedRoute, TargetStop[]> NamedRoutes;

        public ActionResult Index()
        {
            GetRoutes();
            var model = NamedRoutes.Select(p => p.Key.ToString()).ToArray();
            return View(model);
        }

        public async Task<ActionResult> Summary(string routeName)
        {
            GetRoutes();

            NamedRoute namedRoute = NamedRoute.Unknown;

            namedRoute = (NamedRoute)Enum.Parse(typeof(NamedRoute), routeName);

            //namedRoute = NamedRoute.WorkToHome;
            //// todo get named route from command line or use current location
            //if (namedRoute == NamedRoute.Unknown)
            //{
            //    // Determine the most appropriate route based on our current location
            //    var location = GetCurrentLocation();
            //    if (location != null)
            //        namedRoute = GetClosestNamedRoute(location);
            //}
            TargetStop[] targetStops = NamedRoutes[namedRoute];

            // Lookup buses
            var now = DateTime.Now;

            var busInfo = new BusInfo();
            var departures = await busInfo.ListDeparturesAsync(targetStops);

            var model = new TimetableSummary
            {
                UserId = "dan",
                Route = namedRoute,
                TimeStamp = now,
                Departures = departures.Select(ss => MapToDeparture(ss, now)).ToArray(),
            };

            return View(model);
        }

        private static void GetRoutes()
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(folder, @"namedRoutes.json");
            if (System.IO.File.Exists(path))
                NamedRoutes = Utility.LoadFromFile<List<KeyValuePair<NamedRoute, TargetStop[]>>>(path).ToDictionary(r => r.Key, r => r.Value);
            else
                NamedRoutes = NamedRoutes_DJW.BuildNamedRoutes();

            if (NamedRoutes == null || NamedRoutes.Count == 0)
                throw new InvalidOperationException("Need at least one route");
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