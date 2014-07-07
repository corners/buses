using ReadingBusesCore;
using ReadingBusesCore.Entities;
using ReadingBusesCore.Routes;
using ReadingBusesCore.Routes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZeroTwoTwelve.Extensions;

namespace readingBuses.Controllers
{
    public class RouteEditorApiController : Controller
    {
        public JsonResult GetRoute(string name)
        {
            var route = RouteApi.GetRoute(name);
            var model = route.TargetStops.SelectMany(ts => ts.Services, (ts, s) => new {
                LocationId = ts.LocationId, 
                Name = ts.Name, 
                Longitude = ts.Location.Longitude,
                Latitude = ts.Location.Latitude,
                MinutesToLocation = ts.MinutesToLocation, 
                Services = s,
            }).ToArray();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveRoute(Route route)
        {
            try
            {
                var model = RouteApi.SaveRoute(route);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                var error = new Error();
                error.ErrorID = 123; // todo useful code
                error.Level = 2; // todo useful level
                error.Message = ex.Message; // todo useful message

                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetServicesJson()
        {
            var locations = await RouteApi.GetLocations();
            var services = RouteApi.Services(locations);
            var model = services.OrderBy(s => s, new ServiceComparer()).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStopsForServiceJson(string service, int direction)
        {
            try
            {
                var model = await RouteApi.GetSerivcePattern(service);

                var lookup = new Dictionary<string, Location>(StringComparer.OrdinalIgnoreCase);
                // Only populate the dictionary if we have locations to check
                if (model.Locations.Count() > 0)
                {
                    var locations = await RouteApi.GetLocations();
                    lookup = locations.ToDictionary(l => l.ID, StringComparer.OrdinalIgnoreCase);
                }

                var result = new
                {
                    ServiceId = model.ServiceId,
                    Locations = (from location in model.Locations
                                 where location.Direction == direction
                                 let detail = lookup.ValueOrDefault(location.Id, null)
                                 where detail != null
                                 select new
                                 {
                                     Id = location.Id,
                                     Name = detail.Name,
                                     Longitude = detail.Longitude,
                                     Latitude = detail.Latitude,
                                     Direction = location.Direction,
                                     DisplayOrder = location.DisplayOrder,
                                 }
                                 ).OrderBy(o => o.Direction)
                                  .ThenBy(o => o.DisplayOrder).ToArray(),
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                return null;
            }
        }

        //static string DirectionToString(int direction)
        //{
        //    switch (direction)
        //    {
        //        case 0: return "Out of Rdg";
        //        case 1: return "Into Rdg";
        //        default: return "";
        //    }
        //}
    }
}