using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class SuspensionViewModel
    {
        [Required]
        [Display(Name ="Start Date")]
        [DataType(DataType.Date)]
        [DateChecker(ErrorMessage = "Invalid Date selected")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DateChecker(ErrorMessage = "Invalid Date selected")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }

    }
}
