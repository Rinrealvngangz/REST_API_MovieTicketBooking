using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace MovieTicketBookingAPI.Infrastructure.ActionFilter
{
    public class ValidationMovieExistAtrribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public ValidationMovieExistAtrribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = Guid.Empty;

            if (context.HttpContext.Request.Method.Equals("POST"))
            {
                var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dtos")).Value;
                 var item = param as MovieExpand;
                id = Guid.Parse(item.Id);
                var existMovie = await _unitOfWork.MovieRepository.GetByIdAsync(id);
                    if(existMovie is not null)
                {
                    context.Result = new ConflictResult();
                    return;
                }
                await next();
            }

            if (context.ActionArguments.ContainsKey("id"))
            {
                id =(Guid)context.ActionArguments["id"];

            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad parameter");
                return;
            }
             
          var movie =  await _unitOfWork.MovieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            

            if (context.HttpContext.Request.Method.Equals("GET"))
            {
                context.HttpContext.Items.Add("movie", movie);
                await next();
            }

        }
    }
}
