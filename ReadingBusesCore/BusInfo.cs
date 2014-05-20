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

namespace ReadingBusesCore
{
    public class BusInfo
    {
        public SuggestedStop[] ListDepartures(TargetStop[] targetStops)
        {
            return ListDeparturesAsync(targetStops).Result;
        }

        public async Task<SuggestedStop[]> ListDeparturesAsync(TargetStop[] targetStops)
        {
            var tasks = targetStops.Select(ts => CallsAtStop(ts)).ToArray();

            var all = from suggestions in (await Task.WhenAll(tasks))
                      from suggestion in suggestions
                      orderby suggestion.ScheduledDeparture
                      select suggestion;

            return all.ToArray();
        }

        // TODO - tidy up

        static async Task<SuggestedStop[]> CallsAtStop(TargetStop targetStop)
        {
            var targetFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ods.reading-travelinfo.co.uk/");
                client.DefaultRequestHeaders.Accept.Clear();

                var json = new JsonMediaTypeFormatter();
                json.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

                var formatters = new List<MediaTypeFormatter>() {
                    json,
                };

                CallsAtStopResponse callsAtStop = await GetCallsAtStopAsync(targetStop.LocationId, client);

                var buses = from call in callsAtStop.MonitoredLocation.Calls
                            where targetStop.Services.Contains(call.Service)
                            select Map.ToSuggestedStop(targetStop, callsAtStop, call);

                return buses.ToArray();
            }
        }

        static async Task<CallsAtStopResponse> GetCallsAtStopAsync(string locationId, HttpClient client)
        {
            CallsAtStopResponse callsAtStop;

            HttpResponseMessage response = await client.GetAsync("api/1/bus/calls/" + locationId + ".json");
            if (response.IsSuccessStatusCode)
            {
                callsAtStop = await response.Content.ReadAsAsync<CallsAtStopResponse>();
            }
            else throw new ArgumentException("todo");

            return callsAtStop;
        }
    }
}
