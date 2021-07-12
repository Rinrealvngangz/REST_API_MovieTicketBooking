using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extension;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var users =  await _unitOfWork.User.GetAllAsync().AsToListUserDtos();
            if (users == null) return NotFound();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(Guid.Parse(id));
            if (user == null) return NotFound();
            return Ok(user.AsToUserDtos());

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
  
           var result =  await  _unitOfWork.User.UpdateUserAsync(id, userDtos.Password ,User);
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
        [HttpPost]
        [Route("{id}/AddUserRole")]

        public async Task<IActionResult> AddUserRoleAsync(string id ,[FromBody] RoleDtos roleDtos)
        {
           var result = await _unitOfWork.UserRole.AddUserRoleAsync(id, roleDtos.Id.ToString());
            if (result) return Ok();
            return BadRequest();
        }
    }
}
