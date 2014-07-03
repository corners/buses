using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes.Entities
{
    public class Location
    {
        public string ID { get; set; }
        public string Naptan { get; set; }
        public string Name { get; set; }
        public string Bay { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Services { get; set; }

        public IReadOnlyList<string> ServiceList 
        { 
            get
            {
                if (_serviceList == null)
                {
                    _serviceList = (Services ?? "").Split(new[] { '/' })
                                                   .Where(s => !string.IsNullOrEmpty(s))
                                                   .ToList().AsReadOnly();
                }
                return _serviceList;
            }
        }
        IReadOnlyList<string> _serviceList;
    }
}
