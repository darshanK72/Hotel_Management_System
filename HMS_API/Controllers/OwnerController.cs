using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_API.Controllers
{
    [Route("api/owner")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _ownerRepository.GetUsersAsync());
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _ownerRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("user/register")]
        public async Task<ActionResult<User>> Register(UserPayload userPayload)
        {
            try
            {
                var user = await _ownerRepository.RegisterUserAsync(userPayload);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem occurred while registering the user. Please try again.");
            }
        }

        [HttpPut("user/update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserPayload userPayload)
        {
            try
            {
                var updatedUser = await _ownerRepository.UpdateUserAsync(id, userPayload);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                if (!_ownerRepository.UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("user/delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _ownerRepository.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("departments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return Ok(await _ownerRepository.GetDepartmentsAsync());
        }

        [HttpGet("department/{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _ownerRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpPost("department")]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            var createdDepartment = await _ownerRepository.CreateDepartmentAsync(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.DepartmentId }, createdDepartment);
        }

        [HttpPut("department/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            try
            {
                await _ownerRepository.UpdateDepartmentAsync(id, department);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                if (!_ownerRepository.DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("department/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _ownerRepository.DeleteDepartmentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("report")]
        public async Task<ActionResult<Report>> GetReport()
        {
            var report = await _ownerRepository.GetReportAsync();
            return Ok(report);
        }
    }
}
