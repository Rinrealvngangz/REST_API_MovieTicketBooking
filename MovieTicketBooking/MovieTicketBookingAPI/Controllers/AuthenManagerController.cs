using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IConfiguration;
using Core.UnitOfWork;
using MovieTicketBookingAPI.Data.Entities;
using Dtos;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenManagerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenManagerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDtos item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifyEmailDtos = await _unitOfWork.User.Register(item);
            if (verifyEmailDtos == null) return BadRequest();
            return Created("api/VerifyEmail/{userId}/{code}", new { userId = verifyEmailDtos.UserId, code = verifyEmailDtos.Code });

        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {

            return Ok("Hello");
        }


        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDtos verifyEmailDtos)
        {
          
            var result =  await _unitOfWork.User.VerifyEmail(verifyEmailDtos);
            if (result !=null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
