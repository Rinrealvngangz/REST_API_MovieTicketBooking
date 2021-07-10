using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(string id ,[FromBody] UserDtos userDtos)
        {
            var User = new User
            {
                UserName = userDtos.UserName,
                FirstName = userDtos.FirstName,
                LastName = userDtos.LastName,
                Email =userDtos.Email,
                IsVip =userDtos.IsVip
            };
  
           var result =  await  _unitOfWork.User.UpdateAsync(id, userDtos.Password ,User);
                         await _unitOfWork.CompleteAsync();
            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           var result = await _unitOfWork.User.DeleteAsync(Guid.Parse(id));
                        await _unitOfWork.CompleteAsync();
            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
