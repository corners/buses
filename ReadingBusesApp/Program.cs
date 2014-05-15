using Newtonsoft.Json;
using ReadingBusesCore;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesApp
{
    class Program
    {
        static readonly Configuration Config = new Configuration();

        static Dictionary<NamedRoute, TargetStop[]> NamedRoutes;

        static GeoCoordinate GetCurrentLocation()
        {
            return new GeoCoordinate(51.455563, -0.9664306);    // The Blade
        }

        static void Main(string[] args)
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(folder, @"NamedRoutes-DJW.json");
            if (File.Exists(path))
                NamedRoutes = Utility.LoadFromFile<List<KeyValuePair<NamedRoute, TargetStop[]>>>(path).ToDictionary(r => r.Key, r => r.Value);

            if (NamedRoutes == null || NamedRoutes.Count == 0)
                throw new InvalidOperationException("Need at least one route");

            NamedRoute namedRoute = NamedRoute.Unknown;
            // todo get named route from command line or use current location
            if (namedRoute == NamedRoute.Unknown)
            {
                // Determine the most appropriate route based on our current location
                var location = GetCurrentLocation();
                if (location != null)
                    namedRoute = GetClosestNamedRoute(location);
            }
            TargetStop[] targetStops = NamedRoutes[namedRoute];

            // Lookup buses
            var now = DateTime.Now;

            var busInfo = new BusInfo();
            var departures = busInfo.ListDepartures(targetStops);

            // Output
            Console.WriteLine("{1} @ {0:HH:mm:ss} ({0:dd-MMM-yyyy})", now, namedRoute);
            Console.WriteLine("");
            Console.WriteLine("Departs\tStop\tBus\tReachable");
            foreach (var bus in departures)
            {
                Console.WriteLine("{2}\t{1}\t{0} to {3}\t{4}", 
                    bus.Service, 
                    bus.LocationName, 
                    Utility.FriendlyTime(bus.ScheduledDeparture - now), 
                    bus.Destination,
                    Utility.IsReachable(bus.ScheduledDeparture, now, bus.TravelTimeInMinutes, Config.DepartureMarginInSeconds));
            }
            Console.ReadLine();
        }

        private static NamedRoute GetClosestNamedRoute(GeoCoordinate location)
        {
            var distancesToRouteStops = from pair in NamedRoutes
                                        from stop in pair.Value
                                        let distance = location.GetDistanceTo(stop.Location)
                                        orderby distance
                                        select pair.Key;
            return distancesToRouteStops.First();
        }

    }
}
