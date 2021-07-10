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
            return  Ok(new { UserId = verifyEmailDtos.UserId, Code = verifyEmailDtos.Code });

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
            return Ok(user);
        }


        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDtos verifyEmailDtos)
        {
            var existUser =  await _unitOfWork.User.VerifyEmail(verifyEmailDtos);
                            
            if (existUser !=null)
            {
               var response = await  _unitOfWork.Authen.GenerateToken(existUser);
                await _unitOfWork.CompleteAsync();
                return Ok(response);
            }
            return BadRequest();
         
        }

        [HttpPost]
        [Route("RefreshToken")]

        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDtos refreshTokenDtos )
        {
           var response =await _unitOfWork.Authen.VerifyRefreshToken(refreshTokenDtos);
                         await _unitOfWork.CompleteAsync();
            return Ok(response);
        }

    }
}
