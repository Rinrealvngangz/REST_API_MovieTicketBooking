using Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Infrastructure.ActionFilter
{
    public class ValidationFilterAttribute : IActionFilter
    {
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dtos") ).Value;
            if (param == null)
            {
              
                context.Result = new BadRequestObjectResult($"Object is null. Controller {controller}, action {action}.");
            }
            if (param.GetType().FullName.Contains("Movie"))
            {
                var movie = param as MovieExpand;
                  
                DateTime publishYear;
                var validDate = DateTime.TryParseExact(
                  movie.PublishedYear,
                  "MM-dd-yyyy",
                  CultureInfo.InvariantCulture,
                  DateTimeStyles.None,
                  out publishYear);
                if (validDate == false)
                {
                    context.Result = new BadRequestObjectResult("Error format time (MM-dd-yyyy)");
                }
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

    }
}
