using HMS_API.Payload;
using HMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_System.Repository.Interfaces
{
    public interface IOwnerRepository
    {

        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> RegisterUserAsync(UserPayload userDto);
        Task<User> UpdateUserAsync(int id, UserPayload userDto);
        Task<bool> DeleteUserAsync(int id);
        bool UserExists(int id);

        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(int id, Department department);
        Task<bool> DeleteDepartmentAsync(int id);
        bool DepartmentExists(int id);

        Task<Report> GetReportAsync();
        Task<ReservationReport> GenerateReservationReportAsync();
        Task<StaffReport> GenerateStaffReportAsync();
    }
}
