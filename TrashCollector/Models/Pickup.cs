using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class Pickup
    {
        [Key]
        public int Id { get; set; }
        public decimal Balance { get; set; }

        [Required]
        [Display(Name="Pickup Day")]
        public string PickupDay { get; set; }

        [Required]
        [Display(Name= "One Time Pickup")]
        public DateTime OneTimePickup { get; set; }

        [Required]
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name= "End Date")]
        public DateTime EndDate { get; set; }

        public int IsSuspended { get; set; }
    }
}
