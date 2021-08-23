using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Utilities.Extension;
namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
            private readonly IUnitOfWork _unitOfWork;

            public ReservationController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

           [HttpGet("{id}")]
           public async Task<IActionResult> GetById(string id)
           {
             var reservation = await _unitOfWork.Reservation.GetByIdAsync(Guid.Parse(id));
            if (reservation is null) return BadRequest();
            return Ok(reservation.AsToViewReservation());
           }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _unitOfWork.Reservation.GetAllAsync();
            if (reservations is null) return BadRequest();
            return Ok(reservations.AsToViewReservationList());
        }

        [HttpPost]
            public async Task<IActionResult> AddReservation([FromBody] ReservationDtos reservation)
            {
                string userId = _unitOfWork.Reservation.GetIdUserClaim(reservation.Token);
                var newReservation = new Reservation
                {
                    Id = new Guid(),
                    UserId = Guid.Parse(userId),
                    SeatId = reservation.SeatId,
                    ScheduledMovieId = reservation.ScheduledMovieId
                };
                var item = await _unitOfWork.Reservation.AddAsync(newReservation);
               await _unitOfWork.CompleteAsync();
                if (item == null) return BadRequest();
                return Ok(item);
            }

            [HttpPut("{id}")]
           public async Task<IActionResult> Update(string id , [FromBody]ReservationDtos reservation)
           {
            string userId = _unitOfWork.Reservation.GetIdUserClaim(reservation.Token);

            var item = new Reservation
            {
                UserId = Guid.Parse(userId),
                SeatId = reservation.SeatId,
                ScheduledMovieId = reservation.ScheduledMovieId
            };
           var result = await _unitOfWork.Reservation.UpdateAsync(id, item);
            if (result)
            {
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            return BadRequest();

            }
          [HttpDelete("{id}")]
          public async Task<IActionResult> Delete(string id)
          {
           var result = await _unitOfWork.Reservation.DeleteAsync(Guid.Parse(id));
              await _unitOfWork.CompleteAsync();
             if(!result) return BadRequest();
            return NoContent();
          }

        }

    }

