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
    // Optimise initial loading time of Departure pages
    // Optimise route & departure lookup
    // Use location to determine time to stop


    public class DeparturesController : AsyncController
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

        public ActionResult ForRoute(string id)
        {
            var model = new DeparturesForRoute { RouteName = id };
            return View(model);
        }
    }
}