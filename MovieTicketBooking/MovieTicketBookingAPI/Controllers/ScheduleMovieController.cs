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
using Utilities.Extension;
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
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(string id)
        {
           var item = await _unitOfWork.ScheduleMovie.GetByIdAsync(Guid.Parse(id));
            if (item is null) return BadRequest();
            return Ok(item.AsToScheduleMovieViews());
        }
        [HttpGet()]

        public async Task<IActionResult> GetAll()
        {
              var items = await _unitOfWork.ScheduleMovie.GetAllAsync();
            if (items is null) return BadRequest();
            return Ok(items.AsToScheduleMovieViewsList());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (string id ,[FromBody] ScheduleMovieDtos scheduledMovie)
        {
            var isAuditorium = await _unitOfWork.Auditorium.GetByIdAsync(scheduledMovie.AuthoriumId);
            if (isAuditorium == null) return BadRequest("Audirorium not exist");

            var scheduleMov = new ScheduledMovie
            {
                Start = scheduledMovie.Start,
                Price = scheduledMovie.Price,
                AuthoriumId = scheduledMovie.AuthoriumId,
                MovieId = scheduledMovie.MovieId
            };
           

            var result =  await _unitOfWork.ScheduleMovie.UpdateAsync(id ,scheduleMov);
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.ScheduleMovie.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }


    }
}
