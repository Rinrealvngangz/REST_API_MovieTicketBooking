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
        }
    }

