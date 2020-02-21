using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class Pickup
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Bill")]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Pickup Day")]
        public string PickupDay { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateChecker(ErrorMessage ="Invalid Date selected")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "One Time Pickup")]
        public DateTime OneTimePickup { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateChecker(ErrorMessage = "Invalid Date selected")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DateChecker(ErrorMessage = "Invalid Date selected")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name ="Pickup Status")]
        public bool IsSuspended { get; set; }

        [NotMapped]
        public SelectList Days
        {
            get
            {
                return new SelectList(new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
            }
        }
    }

}

