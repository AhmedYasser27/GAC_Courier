using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class ByWayBill: KeyValuePair
    {
        public ICollection<WayBill> CourierRequests { get; set; }

        public ByWayBill()
        {
            CourierRequests = new System.Collections.ObjectModel.Collection<WayBill>();
        }
    }
}
