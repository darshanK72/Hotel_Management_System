using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS_API.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string? ReportType { get; set; }

        public DateTime GeneratedDate { get; set; }
        public ReservationReport? ReservationReport { get; set; }
        public StaffReport? StaffReport { get; set; }
    }

    public class ReservationReport
    {
        public string? Month { get; set; }
        public int TotalNumberOfReservations { get; set; }
        public decimal TotalIncome { get; set; }

        public List<RoomReservation> roomReservations { get; set; }
    }

    public class RoomReservation
    {
        public int? RoomId { get; set; }
        public RoomType? RoomType { get; set; }
        public string? RoomNumber { get; set; }
        public int NumberOfTimeReserved { get; set; }
        public decimal TotalIncomeFromRoom { get; set; }
    }

    public class StaffReport
    {
        public string? Month { get; set; }
        public int TotalNumberOfStaff { get; set; }
        public decimal TotalSalary { get; set; }
        public List<StaffSalary>? staffSalaries { get; set; }
    }

    public class StaffSalary
    {
        public int StaffId { get; set; }
        public string? StaffName { get; set; }
        public decimal Salary { get; set;}
        public string? Department { get; set;}
    }
}
