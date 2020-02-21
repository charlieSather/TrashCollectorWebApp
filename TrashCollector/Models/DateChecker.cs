using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class DateChecker : ValidationAttribute
    {
        public override bool IsValid(object value) => Convert.ToDateTime(value).Date >= DateTime.Now.Date;
    }
}
