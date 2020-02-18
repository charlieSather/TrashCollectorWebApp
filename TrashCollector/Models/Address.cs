using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAdress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }


    }
}
