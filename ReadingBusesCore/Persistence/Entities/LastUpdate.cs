using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Persistence.Entities
{
    public class LastUpdate
    {
        [Key]
        public string TableName { get; set; }
        public DateTime Updated { get; set; }
    }
}
