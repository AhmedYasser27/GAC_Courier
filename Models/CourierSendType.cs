using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class CourierSendType : KeyValuePair
    {

        public ICollection<CourierRequest> CourierRequests { get; set; }

        public CourierSendType()
        {
            CourierRequests = new Collection<CourierRequest>();
        }
    }
}