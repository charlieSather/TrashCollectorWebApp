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
        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [Required]
        public int ZipCode { get; set; }

        public override string ToString() => $"{StreetAddress},{City},{State},{ZipCode}";
        public string StandardAddressFormat() => $"{StreetAddress}, {City}, {State} {ZipCode}";


    }
}
