using HMS_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management_System.Payloads
{
    public class ReservationPayload
    {
        public int? ReservationId { get; set; }

        [Required]
        public int NumberOfAdults { get; set; }

        [Required]
        public int NumberOfChildren { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
    }

}