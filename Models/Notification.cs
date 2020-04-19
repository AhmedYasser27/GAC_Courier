using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Notification : BaseEntity
    {
       
        public DateTime DateTime { get; set; }

        public NotificationType Type { get; set; }

        public CourierRequest CourierRequest { get; set; }
        public int? CourierRequestId { get; set; }


        public WayBill WayBill { get; set; }
        public int? WayBillId { get; set; }

        public Delivery Delivery { get; set; }
        public int? DeliveryId { get; set; }
    }
}
