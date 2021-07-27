using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody]SeatDtos seatDtos)
        {
            if (!ModelState.IsValid) return BadRequest();
            var Seat = new Seat { 
               Id =new Guid(),
               Number =seatDtos.Number,
               Name = seatDtos.Name,
               RowId =seatDtos.RowId
            };

          var item =  await _unitOfWork.Seat.AddAsync(Seat);
            await _unitOfWork.CompleteAsync();
            return Ok(item);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(string id , [FromBody] SeatDtos seatDtos)
        {
            if (!ModelState.IsValid) return BadRequest();
            var Seat = new Seat
            {
                Number = seatDtos.Number,
                Name = seatDtos.Name,
                RowId = seatDtos.RowId
            };

           var result = await _unitOfWork.Seat.UpdateAsync(id,Seat);
           await _unitOfWork.CompleteAsync();
            if (result) return NoContent();

            return BadRequest();
            
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(string id)
        {
           var result = await _unitOfWork.Seat.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (result) return NoContent();

            return BadRequest();
        }

    }
}
