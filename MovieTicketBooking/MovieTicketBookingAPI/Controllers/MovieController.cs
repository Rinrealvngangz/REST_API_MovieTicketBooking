using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Dtos;
using MovieTicketBookingAPI.Infrastructure.ActionFilter;
using System.Globalization;
using Utilities.Extension;
using System.Security.Policy;
using Utilities.Extension.DataShaping;
using Dtos.Request;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationMovieExistAtrribute))]
        public async Task<IActionResult> CreateMovie([FromBody] MovieExpand movie)
        {
     
            var newMovie = new Movie
            {
                Id = new Guid(),
                Name = movie.Name,
                Minutes = new TimeSpan(movie.Hours, movie.Minutes, movie.Second),
                Description = movie.Description,
                PublishedYear = DateTime.Parse(movie.PublishedYear)
            };
          var item =  await _unitOfWork.MovieRepository.AddAsync(newMovie);
            await _unitOfWork.CompleteAsync();
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get) ,new { id = item.Id } ,item.AsToMovieDtos() );
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidationMovieExistAtrribute))]
        public IActionResult Get(Guid id)
        {
            var item =  HttpContext.Items["movie"] as Movie;
            return Ok(item.AsToMovieDtos());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MovieParameters movieParameters)
        {
           var items = await _unitOfWork.MovieRepository.GetAllAsync();
           if(items == null) return BadRequest();
            var result = PagedList<Movie>.ToPagedList(items, movieParameters.PageNumber,movieParameters.PageSize)
                                         .Filter(movieParameters.MinDate, movieParameters.MaxDate)
                                         .Sort(movieParameters.Sort)
                                         .Search(movieParameters.Search)
                                         .AsToMovieDtosList()
                                         .Shaper(movieParameters.Fields);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationMovieExistAtrribute))]
        public async Task<IActionResult> Update(Guid id,[FromBody]MovieExpand movie)
        {
            var movieUpdate = new Movie
            {
                Name = movie.Name,
                Minutes = new TimeSpan(movie.Hours, movie.Minutes, movie.Second),
                Description = movie.Description,
                PublishedYear = DateTime.Parse(movie.PublishedYear)
            };
           var result = await _unitOfWork.MovieRepository.UpdateAsync(id.ToString(), movieUpdate);
            await _unitOfWork.CompleteAsync();
           if(!result) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidationMovieExistAtrribute))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = HttpContext.Items["movie"] as Movie;
           var result = await _unitOfWork.MovieRepository.DeleteAsync(item.Id);
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }

    }
}
