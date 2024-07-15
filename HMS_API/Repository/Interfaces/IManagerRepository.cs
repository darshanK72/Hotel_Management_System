using HMS_API.Models;
using HMS_API.Payload;

namespace Hotel_Management_System.Repository.Interfaces
{
    public interface IManagerRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> CreateRoomAsync(Room room);
        Task<Room> UpdateRoomAsync(int id, Room room);
        Task<bool> DeleteRoomAsync(int id);
        bool RoomExists(int id);

        Task<IEnumerable<Staff>> GetStaffsAsync();
        Task<Staff> GetStaffByIdAsync(int id);
        Task<Staff> CreateStaffAsync(StaffPayload staffDto);
        Task<Staff> UpdateStaffAsync(int id, StaffPayload staffDto);
        Task<bool> DeleteStaffAsync(int id);
        bool StaffExists(int id);
    }
}
