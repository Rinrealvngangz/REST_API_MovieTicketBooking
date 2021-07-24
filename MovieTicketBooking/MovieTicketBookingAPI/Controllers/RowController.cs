using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Utilities.Extension;
using Microsoft.Extensions.DependencyInjection;
namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RowController(IServiceProvider serviceProvider)
        {
            _unitOfWork = (IUnitOfWork)serviceProvider.GetRequiredService(typeof(IUnitOfWork));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRow(string id)
        {
          var item = await _unitOfWork.Row.GetByIdAsync(Guid.Parse(id));
            if (item == null) return BadRequest();
            return Ok(item.AsToRowWithAuditoriumDtos());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRow()
        {
            var item = await _unitOfWork.Row.GetAllAsync();
            
            if (item == null) return BadRequest();
            return Ok(JsonSerializer.Serialize(item.AsToRowsDtosList()));
        }


        [HttpPost]

        public async Task<IActionResult> CreateRow(RowDtos rowDtos)
        {
            if (!ModelState.IsValid)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            var existAuditorium = await _unitOfWork.Auditorium.GetByIdAsync(rowDtos.AuditoriumId);
            if (existAuditorium is null) return BadRequest("No exist Auditorium");

            var row = new Row
            {
                Id = new Guid(),
                Number = rowDtos.Number,
                AuditoriumId = existAuditorium.Id
            };
          var item = await _unitOfWork.Row.AddAsync(row);
            await _unitOfWork.CompleteAsync();
            if (item == null) return BadRequest();

            return CreatedAtAction(nameof(GetRow), new { id = item.Id }, item.AsToRowDtos());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RowDtos rowDtos)
        {
            var row = new Row
            {
                Number = rowDtos.Number,
                AuditoriumId = rowDtos.AuditoriumId
            };
          var result = await _unitOfWork.Row.UpdateAsync(id,row);
            await _unitOfWork.CompleteAsync();
            if (result) return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           var result = await _unitOfWork.Row.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (result) return NoContent();
            return BadRequest();
        }
    }
}
