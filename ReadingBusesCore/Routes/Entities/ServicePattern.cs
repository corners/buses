using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes.Entities
{
    public class ServicePattern
    {
        [Key]
        public string ServiceId { get; set; }
        public ServiceLocation[] Locations { get; set; }
    }
}
