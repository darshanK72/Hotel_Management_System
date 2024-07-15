using HMS_API.Models;
using Microsoft.EntityFrameworkCore;

public class HMSDbContext : DbContext
{
    public HMSDbContext(DbContextOptions<HMSDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, UserRole = "Owner", Description = "Hotel owner" },
            new Role { RoleId = 2, UserRole = "Manager", Description = "Hotel manager" },
            new Role { RoleId = 3, UserRole = "Receptionist", Description = "Front desk staff" }
        );

        modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, Name = "Management", Description = "Staff controlling management activities of hotel" },
            new Department { DepartmentId = 2, Name = "Reception", Description = "Staff controlling reception" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Username = "Hotel_Owner", Password = "owner123", Email = "owner@example.com", RoleId = 1, DepartmentId = 1 },
            new User { UserId = 2, Username = "Hotel_Manager", Password = "manager123", Email = "manager@example.com", RoleId = 2, DepartmentId = 1 },
            new User { UserId = 3, Username = "Hotel_Reception", Password = "reception123", Email = "reception@example.com", RoleId = 3, DepartmentId = 2 }
        );
    }
}
