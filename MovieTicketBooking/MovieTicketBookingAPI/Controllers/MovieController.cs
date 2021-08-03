using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Dtos;
using System.Globalization;
using Utilities.Extension;
using System.Security.Policy;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieExpand movie)
        {
          
            if (!ModelState.IsValid) return BadRequest();
            DateTime publishYear;    
         var validDate =   DateTime.TryParseExact(
           movie.PublishedYear,
           "MM-dd-yyyy",
           CultureInfo.InvariantCulture,
           DateTimeStyles.None,
           out publishYear);
            if (validDate == false) return BadRequest("Error format time (MM-dd-yyyy)");
      
            var newMovie = new Movie
            {
                Id = new Guid(),
                Name = movie.Name,
                Minutes = new TimeSpan(movie.Hours, movie.Minutes, movie.Second),
                Description = movie.Description,
                PublishedYear = publishYear
            };
          var item =  await _unitOfWork.MovieRepository.AddAsync(newMovie);
            await _unitOfWork.CompleteAsync();
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get) ,new { id = item.Id } ,item.AsToMovieDtos() );
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(string id)
        {
          var item = await  _unitOfWork.MovieRepository.GetByIdAsync(Guid.Parse(id));
          if(item == null)  return BadRequest();
            return Ok(item.AsToMovieDtos());
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var items = await _unitOfWork.MovieRepository.GetAllAsync();
           if(items == null) return BadRequest();
           return Ok(items.AsToMovieDtosList());
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(string id,[FromBody]MovieExpand movie)
        {
            if (!ModelState.IsValid) return BadRequest();

            DateTime publishYear;
            var validDate = DateTime.TryParseExact(
              movie.PublishedYear,
              "MM-dd-yyyy",
              CultureInfo.InvariantCulture,
              DateTimeStyles.None,
              out publishYear);
            if (validDate == false) return BadRequest("Error format time (MM-dd-yyyy)");


            var movieUpdate = new Movie
            {
                Name = movie.Name,
                Minutes = new TimeSpan(movie.Hours, movie.Minutes, movie.Second),
                Description = movie.Description,
                PublishedYear = publishYear
            };
           var result = await _unitOfWork.MovieRepository.UpdateAsync(id, movieUpdate);
            await _unitOfWork.CompleteAsync();
           if(!result) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           var result = await _unitOfWork.MovieRepository.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }

    }
}
