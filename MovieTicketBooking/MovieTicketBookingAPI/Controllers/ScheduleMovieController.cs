using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Dtos;
using System.Text.Json;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleMovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleMovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleMovieDtos scheduledMovie )
        {
            var sheduleMov = new ScheduledMovie
            {
                Id = new Guid(),
                Start = scheduledMovie.Start,
                Price = scheduledMovie.Price,
                AuthoriumId = scheduledMovie.AuthoriumId,
                MovieId = scheduledMovie.MovieId
            };
            var isAuditorium = await _unitOfWork.Auditorium.GetByIdAsync(scheduledMovie.AuthoriumId);
            if (isAuditorium == null) return BadRequest("Audirorium not exist");
            var item =  await _unitOfWork.ScheduleMovie.AddAsync(sheduleMov);
            await _unitOfWork.CompleteAsync();
            if (item == null) return BadRequest();
            return Ok(JsonSerializer.Serialize(item));
        }
    }
}
