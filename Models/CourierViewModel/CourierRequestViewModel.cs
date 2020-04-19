using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models.CourierViewModel
{
    public class CourierRequestViewModel
    {
        public int? CourierId { get; set; }
      
       
        public string CourierNumber { get; set; }
    }
}
