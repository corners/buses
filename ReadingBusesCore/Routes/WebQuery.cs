using Newtonsoft.Json;
using ReadingBusesCore.Persistence;
using ReadingBusesCore.Persistence.Entities;
using ReadingBusesCore.Routes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes
{
    internal class WebQuery
    {
        //public async Task<IReadOnlyList<ServicePattern>> GetServicePatterns()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        // json api seems to return xml :-(
        //        HttpResponseMessage response = await ReadAsAsync(client, "api/1/bus/servicepatterns");
        //        var result = await response.Content.ReadAsAsync<GetServicePatternResult>();
        //        return result.Root.ServicePatterns.ToList().AsReadOnly();
        //    }
        //}

        public async Task<IReadOnlyList<Location>> GetLocations()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await ReadAsAsync(client, "api/1/bus/locations.json");
                var result = await response.Content.ReadAsAsync<GetLocationsResult>();
                return result.Root.Locations.ToList().AsReadOnly();
            }
        }

        private async Task<HttpResponseMessage> ReadAsAsync(HttpClient client, string requestUri)
        {
            client.BaseAddress = new Uri("http://ods.reading-travelinfo.co.uk/");
            client.DefaultRequestHeaders.Accept.Clear();

            var json = new JsonMediaTypeFormatter();
            json.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            var formatters = new List<MediaTypeFormatter>() { json };

            //return client.GetAsync(requestUri);
            HttpResponseMessage response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
                throw new ArgumentException("todo");

        }
    }
}
