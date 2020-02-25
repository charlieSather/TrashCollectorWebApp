using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;

namespace TrashCollector.ActionFilter
{
    public class SuspensionDates : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionArguments.ContainsKey("customerViewModel"))
            {
                var result = context.ActionArguments["customerViewModel"] as CustomerViewModel;
                var start = result.Pickup.StartDate;
                var end = result.Pickup.EndDate;

                if (start.Date >= end.Date)
                {
                    context.ModelState.AddModelError("Pickup.StartDate", "Start date can not be the same or past the end date");
                }
            }
            else if (context.ActionArguments.ContainsKey("svm"))
            {
                var result = context.ActionArguments["svm"] as SuspensionViewModel;
                var start = result.StartDate;
                var end = result.EndDate;

                if (start.Date >= end.Date)
                {
                    context.ModelState.AddModelError("StartDate", "Start date can not be the same or past the end date");
                }
            }

          
        }
    }
}
