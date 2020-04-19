using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class CourierRequest: BaseEntity
    {
     
        
        [Display(Name = "Number Courier")]
        public int? CourierNumber { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "FM Courier")]
        public string CourierFM { get; set; }

        
        
        public CourierSendType CourierSendType { get; set; }
        [Required]
        [Display(Name = "CourierSendType")]
        public int CourierSendTypeId { get; set; }


        [Display(Name = "Quantity Courier")]
        [Required]
        public int Quantity { get; set; }


        public CourierDescription CourierDescription { get; set; }
        [Required]
        [Display(Name = "Description Courier")]
        public int CourierDescriptionId { get; set; }

      

        public CourierDestination CourierDestination { get; set; }
        [Display(Name = "Destination Courier")]
        [Required]
        public int  CourierDestinationId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "ReciverName")]
        public string ReciverName { get; set; }


        [Required]
        [StringLength(255)]
        [Display(Name = "ReciverContactPerson")]
        public string ReciverContactPerson { get; set; }

        [Display(Name = "Vessel's Name")]
        public string VesselsName { get; set; }
        [Display(Name = "Jop Number")]
        public string JopPerson { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ICollection<CourierRequestWayBill> WayBill { get; set; }

    }
}
