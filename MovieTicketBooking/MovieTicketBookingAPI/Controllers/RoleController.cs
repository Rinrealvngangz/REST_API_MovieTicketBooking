using Core.IConfiguration;
using Dtos;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var listRoles = await _unitOfWork.Role.GetAllAsync().AsToListRoleDtos();
            if (listRoles == null) return BadRequest();
            return Ok(listRoles);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(Guid.Parse(id));
            if (role == null) return BadRequest();
            return Ok(role.AsToRoleDtos());
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDtos roleDtos)
        {
            var role = new Role
            {
                Name = roleDtos.Name.ToLower(),
                Description = roleDtos.Description
            };
            var roleIsSuccess = await _unitOfWork.Role.AddAsync(role);
            if (roleIsSuccess == null) return BadRequest();
            return Ok(roleIsSuccess.AsToRoleDtos());
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(string id, [FromBody] RoleDtos roleDtos)
        {
            var role = new Role
            {
                Name = roleDtos.Name.ToLower(),
                Description = roleDtos.Description
            };
            var result = await _unitOfWork.Role.UpdateAsync(id, role);

            if (!result) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete (Guid id)
        {
           var  result = await  _unitOfWork.Role.DeleteAsync(id);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
