using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class CourierDestination : KeyValuePair
    {

        public ICollection<CourierRequest> CourierRequests { get; set; }

        public CourierDestination()
        {
            CourierRequests = new Collection<CourierRequest>();
        }
    }
}
