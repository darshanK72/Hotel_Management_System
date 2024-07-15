using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_System.Repository.Implementation
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly HMSDbContext _context;

        public ManagerRepository(HMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> UpdateRoomAsync(int id, Room room)
        {
            if (id != room.RoomId)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return false;
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }

        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return await _context.Staffs.Include(s => s.Department).ToListAsync();
        }

        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            return await _context.Staffs.Include(s => s.Department).FirstOrDefaultAsync(s => s.StaffId == id);
        }

        public async Task<Staff> CreateStaffAsync(StaffPayload staffDto)
        {
            var department = await _context.Departments.FindAsync(staffDto.DepartmentId);
            if (department == null)
            {
                throw new InvalidOperationException("Invalid department specified.");
            }

            var staff = new Staff
            {
                Name = staffDto.Name,
                Age = staffDto.Age,
                Address = staffDto.Address,
                NIC = staffDto.NIC,
                Salary = staffDto.Salary,
                Designation = staffDto.Designation,
                Email = staffDto.Email,
                Code = staffDto.Code,
                DepartmentId = staffDto.DepartmentId,
                Department = department
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff> UpdateStaffAsync(int id, StaffPayload staffDto)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                throw new InvalidOperationException("Staff not found.");
            }

            var department = await _context.Departments.FindAsync(staffDto.DepartmentId);
            if (department == null)
            {
                throw new InvalidOperationException("Invalid department specified.");
            }

            staff.Name = staffDto.Name;
            staff.Age = staffDto.Age;
            staff.Address = staffDto.Address;
            staff.NIC = staffDto.NIC;
            staff.Salary = staffDto.Salary;
            staff.Designation = staffDto.Designation;
            staff.Email = staffDto.Email;
            staff.Code = staffDto.Code;
            staff.DepartmentId = staffDto.DepartmentId;
            staff.Department = department;

            _context.Entry(staff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<bool> DeleteStaffAsync(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return false;
            }

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.StaffId == id);
        }
    }
}