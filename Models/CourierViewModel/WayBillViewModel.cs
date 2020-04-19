using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models.CourierViewModel
{
    public class WayBillViewModel
    {
        public int Id { get; set; }
       
        public string[] CourierId { get; set; }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        public bool IsFully { get; set; }

        public int WaybillNumber { get; set; }

        public ByWayBill ByWayBill { get; set; }
        public int ByWayBillId { get; set; }

      
        public CourierDestination CourierDestination { get; set; }

        public int CourierDestinationId { get; set; }
    }
}
