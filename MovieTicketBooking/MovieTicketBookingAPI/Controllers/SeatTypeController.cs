using Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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



    }
}
