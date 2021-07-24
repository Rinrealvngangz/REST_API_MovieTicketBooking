using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Utilities.Extension;
using Dtos;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriumController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditoriumController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuditorium(Guid id)
        {
            var item = await _unitOfWork.Auditorium.GetByIdAsync(id);
        
            if (item == null) BadRequest();
            return Ok(item.AsToAuditoriumDtos());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAuditorium()
        {
         
            var item = await _unitOfWork.Auditorium.GetAllAsync();
            if (item == null) BadRequest();
            return Ok(item.AsToAuditoriumViewDtos());
        }


        [HttpPost]

        public async Task<ActionResult> CreateAuditorium([FromBody] AuditoriumDtos auditoriumDtos)
        {
            var newAuditorium = new Auditorium
            {
                Id = new Guid(),
                Name = auditoriumDtos.Name,
                Capacity = auditoriumDtos.Capacity

            };
          var item =  await _unitOfWork.Auditorium.AddAsync(newAuditorium);
            if (item == null) BadRequest();
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetAuditorium), new { id = item.Id }, item);
        }

        [HttpPut("{id}")] 
        public async Task<ActionResult> Update(string id , [FromBody] Auditorium auditorium)
        {
            var result = await _unitOfWork.Auditorium.UpdateAsync(id, auditorium);
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Auditorium.DeleteAsync(Guid.Parse(id));
            await _unitOfWork.CompleteAsync();
            if (!result) return BadRequest();
            return NoContent();
        }

        

        
    }
}
