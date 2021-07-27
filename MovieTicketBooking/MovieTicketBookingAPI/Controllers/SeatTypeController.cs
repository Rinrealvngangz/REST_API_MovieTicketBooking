using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Utilities.Extension;
namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeatTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetSeatType(string id)
        {
            var item = await _unitOfWork.SeatType.GetByIdAsync(Guid.Parse(id));
            if (item == null) return NotFound();
            return Ok(item.AsToSeatTypeDtos());
        }

        [HttpGet]

        public async Task<IActionResult> GetAllSeatType()
        {
            var item = await _unitOfWork.SeatType.GetAllAsync();
            if (item == null) return NotFound();
            return Ok(item.AsToSeatTypeDtosList());
        }


        [HttpPost]
        public async Task<IActionResult> CreateSeatType([FromBody] SeatType seatType)
        {
            var type = new SeatType
            {
                Id = new Guid(),
                Name = seatType.Name

            };
            var item = await _unitOfWork.SeatType.AddAsync(type);
            await _unitOfWork.CompleteAsync();
            if (item == null) return NotFound();
            return CreatedAtAction(nameof(GetSeatType), new { id = item.Id }, item.AsToSeatTypeDtos());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatType(string id, [FromBody] SeatType seatType)
        {
            var result = await _unitOfWork.SeatType.UpdateAsync(id, seatType);
            await _unitOfWork.CompleteAsync();
            if (result) return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatType(string id)
        {
          var result = await _unitOfWork.SeatType.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (result) return NoContent();
            return BadRequest();
        }
    }
}