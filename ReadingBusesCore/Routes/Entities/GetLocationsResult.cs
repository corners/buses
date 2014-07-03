using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes.Entities
{
    public class GetLocationsResult
    {
        public GetLocationsResult()
        {
            Root = new ResponseRoot();
        }

        public ResponseRoot Root { get; set; }
    }
}
