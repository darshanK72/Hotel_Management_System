using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management_System.Payloads
{
    public class BillPayload
    {
        public int BillId { get; set; }

        [Required]
        [MaxLength(20)]
        public string BillingNumber { get; set; }

        [Required]
        public string StayDates { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Taxes { get; set; }

        [Required]
        [MaxLength(500)]
        public string Services { get; set; }

        public int GuestId { get; set; }

        public int ReservationId { get; set; }

        public string Status {get;set;}
    }
}
