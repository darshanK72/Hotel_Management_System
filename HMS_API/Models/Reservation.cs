using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS_API.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }

        [Required]
        public int NumberOfAdults { get; set; }

        [Required]
        public int NumberOfChildren { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Day { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfNights { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Status { get; set; }
        public int? RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }
        public int? RateId { get; set; }

        [ForeignKey(nameof(RateId))]
        public Rate? Rate { get; set; }
        public int? PaymentId { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public Payment? Payment { get; set; }
        public int? BillId { get; set; }

        [ForeignKey(nameof(BillId))]
        public Bill? Bill { get; set; }

    }

}
