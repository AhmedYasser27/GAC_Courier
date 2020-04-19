using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Delivery : BaseEntity
    {
        public string From { get; set; }

        public WayBill WayBill { get; set; }
        public int WayBillId { get; set; }

        public CourierDestination CourierDestination { get; set; }
        public int CourierDestinationId { get; set; }

        public string To { get; set; }

        public DateTime Date { get; set; }

        public bool IsFullyDeliverd { get; set; }


        public ApplicationUser User { get; set; }
        public string UserId { get; set; }


    }
}
