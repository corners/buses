using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes.Entities
{
    public class ResponseRoot
    {
        public ResponseRoot()
        {
            Locations = new Location[] {};
        }

        public Location[] Locations { get; set; }
    }
}
