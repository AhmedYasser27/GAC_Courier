using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class CourierRequestWayBill 
    {
        public int CourierRequestId { get; set; }
        public int WayBillId { get; set; }

        public CourierRequest CourierRequest { get; set; }
        public WayBill WayBill { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
