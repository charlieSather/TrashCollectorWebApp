using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class EmployeeViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public Employee Employee { get; set; }
        public string Day { get; set; }
        public bool HidePickupTrash { get; set; }
        public SelectList Days
        {
            get
            {
                return new SelectList(new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
            }
        }
    }
}
