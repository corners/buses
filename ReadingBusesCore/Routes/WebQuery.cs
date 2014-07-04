using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Xml;

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

        public async Task<ServicePattern> GetServicePattern(string service)
        {
            using (var client = new HttpClient())
            {
                // Only seems to support XML
                var uri = @"api/1/bus/servicepatterns?service=" + service;
                HttpResponseMessage response = await ReadAsAsync(client, uri);
                var xml = await response.Content.ReadAsStringAsync();
                return ParseServicePatterns(xml).FirstOrDefault();
            }
        }

        static ServiceLocation ParseServiceLocationNode(XmlNode node)
        {
            return new ServiceLocation
            {
                Id = node.SelectSingleNode("Id").InnerText,
                Direction = int.Parse(node.SelectSingleNode("Direction").InnerText),
                DisplayOrder = int.Parse(node.SelectSingleNode("DisplayOrder").InnerText),
            };
        }

        private static List<ServicePattern> ParseServicePatterns(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var xpath = @"//Root/ServicePatterns/ServicePattern";
            var result = doc.SelectNodes(xpath)
                            .OfType<XmlNode>()
                            .Select(node => new ServicePattern
                            {
                                ServiceId = node.SelectSingleNode("ServiceId").InnerText,
                                Locations = node.SelectNodes("Locations/Location")
                                                .OfType<XmlNode>()
                                                .Select(ParseServiceLocationNode)
                                                .ToArray()
                            })
                            .ToList();

            return result;                
        }

        //private static List<Location> ParseServicePatternsResponse(string json)
        //{
        //    var obj = JObject.Parse(json);
        //    var tokens = obj["Root"]["Locations"].ToList();
        //    var result = (from token in tokens
        //                  let sp = MapToLocation(token)
        //                  select sp).ToList();
        //    return result;
        //}

        //static Location MapToLocation(JToken obj)
        //{
        //    return new Location
        //    {
        //        ID = obj["Id"].Value<string>(),
        //        Bay = obj["Bay"].Value<string>(),
        //        Latitude = obj["Latitude"].Value<double>(),
        //        Longitude = obj["Longitude"].Value<double>(),
        //        Name = obj["Name"].Value<string>(),
        //        Naptan = obj["Naptan"].Value<string>(),
        //        Services = obj["Services"].Value<string>(),
        //    };
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
