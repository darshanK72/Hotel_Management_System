using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_API.Controllers
{
    [Route("api/manager")]
    [ApiController]
    [Authorize(Roles = "Manager, Owner")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerController(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        // GET: api/manager/room
        [HttpGet("room")]
        
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return Ok(await _managerRepository.GetRoomsAsync());
        }

        // GET: api/manager/room/{id}
        [HttpGet("room/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _managerRepository.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // POST: api/manager/room
        [HttpPost("room")]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            var createdRoom = await _managerRepository.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.RoomId }, createdRoom);
        }

        // PUT: api/manager/room/{id}
        [HttpPut("room/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, Room room)
        {
            if (id != room.RoomId)
            {
                return BadRequest();
            }

            try
            {
                await _managerRepository.UpdateRoomAsync(id, room);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                if (!_managerRepository.RoomExists(id))
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

        // DELETE: api/manager/room/{id}
        [HttpDelete("room/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _managerRepository.DeleteRoomAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/manager/staff
        [HttpGet("staff")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffs()
        {
            return Ok(await _managerRepository.GetStaffsAsync());
        }

        // GET: api/manager/staff/{id}
        [HttpGet("staff/{id}")]
        public async Task<ActionResult<Staff>> GetStaff(int id)
        {
            var staff = await _managerRepository.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        // POST: api/manager/staff
        [HttpPost("staff")]
        public async Task<ActionResult<Staff>> CreateStaff(StaffPayload staffDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdStaff = await _managerRepository.CreateStaffAsync(staffDto);
                return CreatedAtAction(nameof(GetStaff), new { id = createdStaff.StaffId }, createdStaff);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to create staff. Please try again.");
            }
        }

        // PUT: api/manager/staff/{id}
        [HttpPut("staff/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, StaffPayload staffDto)
        {
            if (id != staffDto.StaffId)
            {
                return BadRequest("Staff ID mismatch.");
            }

            try
            {
                await _managerRepository.UpdateStaffAsync(id, staffDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                if (!_managerRepository.StaffExists(id))
                {
                    return NotFound("Staff not found.");
                }
                else
                {
                    return StatusCode(500, "Failed to update staff. Please try again.");
                }
            }
        }

        // DELETE: api/manager/staff/{id}
        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await _managerRepository.DeleteStaffAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
