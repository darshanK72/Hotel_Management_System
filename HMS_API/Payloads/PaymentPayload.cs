using HMS_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management_System.Payloads
{
    public class PaymentPayload
    {
        public int? PaymentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CreditCardDetails { get; set; }
        public int? BillId { get; set; }

        public int? GuestId { get; set; }
        public int? ReservationId { get; set; }


    }
}
