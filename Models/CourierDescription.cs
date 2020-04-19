using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace coderush.Models
{
    public class CourierDescription : KeyValuePair
    {
     
        public ICollection<CourierRequest> CourierRequests { get; set; }

        public CourierDescription()
        {
            CourierRequests = new Collection<CourierRequest>();
        }
    }
}
