using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes.Entities
{
    public class ServiceLocation
    {
        public string Id { get; set; }
        public int Direction { get; set; }
        public int DisplayOrder { get; set; }
    }
}
