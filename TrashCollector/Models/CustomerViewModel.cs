using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class CustomerViewModel
    {
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public Address Address { get; set; }
        public Pickup Pickup { get; set; }
    }
}
