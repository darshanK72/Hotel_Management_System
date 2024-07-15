using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_System.Repository.Implementation
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly HMSDbContext _context;

        public OwnerRepository(HMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).Include(u => u.Department).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Role).Include(u => u.Department).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> RegisterUserAsync(UserPayload userPayload)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userPayload.Username))
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == userPayload.Email))
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var role = await _context.Roles.FindAsync(userPayload.RoleId);
            if (role == null)
            {
                throw new InvalidOperationException("Invalid role specified.");
            }

            var user = new User
            {
                Username = userPayload.Username,
                Email = userPayload.Email,
                Password = userPayload.Password,
                RoleId = userPayload.RoleId,
                Role = role
            };

            var department = await _context.Departments.FindAsync(userPayload.DepartmentId);
            if (department == null) throw new InvalidOperationException("Invalid department specified.");
            user.DepartmentId = userPayload.DepartmentId;
            user.Department = department;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Password = null;
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, UserPayload userPayload)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == userPayload.Username && u.UserId != id))
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == userPayload.Email && u.UserId != id))
            {
                throw new InvalidOperationException("Email is already registered to another user.");
            }

            user.Username = userPayload.Username;
            user.Email = userPayload.Email;

            if (!string.IsNullOrWhiteSpace(userPayload.Password))
            {
                user.Password = userPayload.Password;
            }

            if (user.RoleId != userPayload.RoleId)
            {
                var newRole = await _context.Roles.FindAsync(userPayload.RoleId);
                if (newRole == null)
                {
                    throw new InvalidOperationException("Invalid role specified.");
                }
                user.RoleId = userPayload.RoleId;
                user.Role = newRole;
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateDepartmentAsync(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                throw new InvalidOperationException("ID mismatch");
            }

            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }

        public async Task<Report> GetReportAsync()
        {
            var reservationReport = await GenerateReservationReportAsync();
            var staffReport = await GenerateStaffReportAsync();

            return new Report
            {
                ReportType = "Monthly",
                GeneratedDate = DateTime.Now,
                ReservationReport = reservationReport,
                StaffReport = staffReport
            };
        }

        public async Task<ReservationReport> GenerateReservationReportAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Payment)
                .ToListAsync();

            var groupedReservations = reservations
                .GroupBy(r => r.RoomId)
                .Select(grp => new RoomReservation
                {
                    RoomId = grp.Key,
                    RoomType = grp.First().Room.RoomType,
                    RoomNumber = grp.First().Room.RoomNumber,
                    NumberOfTimeReserved = grp.Count(),
                    TotalIncomeFromRoom = grp.Sum(r => r.Payment != null ? r.Payment.TotalAmount : 0)
                }).ToList();

            var totalReservations = reservations.Count;
            var totalIncome = reservations.Sum(r => r.Payment != null ? r.Payment.TotalAmount : 0);

            return new ReservationReport
            {
                Month = DateTime.Now.ToString("MMMM"),
                TotalNumberOfReservations = totalReservations,
                TotalIncome = totalIncome,
                roomReservations = groupedReservations
            };
        }

        public async Task<StaffReport> GenerateStaffReportAsync()
        {
            var staff = await _context.Staffs
                .Include(s => s.Department)
                .ToListAsync();

            var totalNumberOfStaff = staff.Count;
            var totalSalary = staff.Sum(s => s.Salary);

            var staffSalaries = staff.Select(s => new StaffSalary
            {
                StaffId = s.StaffId,
                StaffName = s.Name,
                Salary = s.Salary,
                Department = s.Department.Name
            }).ToList();

            return new StaffReport
            {
                Month = DateTime.Now.ToString("MMMM"),
                TotalNumberOfStaff = totalNumberOfStaff,
                TotalSalary = totalSalary,
                staffSalaries = staffSalaries
            };
        }
    }
}