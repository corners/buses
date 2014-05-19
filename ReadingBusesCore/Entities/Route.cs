using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Entities
{
    public class Route
    {
        [Key]
        public string Name { get; set; }

        [NotMapped]
        public TargetStop[] TargetStops { get; set; }

        public string TargetStopsJson
        {
            get
            {
                return Utility.SaveToString(TargetStops);
            }
            set 
            {
                TargetStops = Utility.LoadFromString<TargetStop[]>(value);
            }
        }

    }
}
