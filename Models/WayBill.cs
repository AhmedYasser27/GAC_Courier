using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class WayBill :BaseEntity
    {
       
       
        public DateTime Date { get; set; }
        public bool IsFully { get; set; }

        public int WaybillNumber { get; set; }

        public ByWayBill ByWayBill { get; set; }
        public int  ByWayBillId { get; set; }

        public CourierDestination CourierDestination { get; set; }
        public int CourierDestinationId { get; set; }

        public ICollection<CourierRequestWayBill> CourierRequest { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public WayBill()
        {
            CourierRequest = new Collection<CourierRequestWayBill>();
        }
    }
}
