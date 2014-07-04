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
        public async Task<JsonResult> GetServicesJson()
        {
            var locations = await RouteApi.GetLocations();
            var services = RouteApi.Services(locations);
            var model = services.OrderBy(s => s).ToList();
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

        static string DirectionToString(int direction)
        {
            switch (direction)
            {
                case 0: return "Out of Rdg";
                case 1: return "Into Rdg";
                default: return "";
            }
        }
    }
}