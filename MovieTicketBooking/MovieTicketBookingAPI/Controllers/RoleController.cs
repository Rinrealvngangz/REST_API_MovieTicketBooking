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
    public class RoleController : ControllerBase 
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDtos roleDtos)
        {
            var role = new Role
            {
                Name = roleDtos.Name.ToLower(),
                Description =roleDtos.Description
            };
          var roleIsSuccess =  await  _unitOfWork.Role.AddAsync(role);
            if (roleIsSuccess == null) return BadRequest();
            return Ok(roleIsSuccess.AsToRoleDtos());
         }

    }
}
