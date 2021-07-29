using Core.IConfiguration;
using Dtos;
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
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDtos item)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                return BadRequest(errors);
            }
            var user = await _unitOfWork.User.Login(item);
            var responseJwtToken = await _unitOfWork.Authen.GenerateToken(user);
            return Ok(responseJwtToken);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDtos item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifyEmailDtos = await _unitOfWork.User.RegisterCustomer(item);
            await _unitOfWork.CompleteAsync();
            if (verifyEmailDtos == null) return BadRequest(); 
            return Ok(new { UserId = verifyEmailDtos.UserId, Code = verifyEmailDtos.Code });

        }

    }
}
