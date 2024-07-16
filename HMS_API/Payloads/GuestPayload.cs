using HMS_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS_API.Payload
{
    public class GuestPayload
    {
        public int? GuestId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        //[Phone(ErrorMessage ="Invalid Phone Number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? MemberCode { get; set; }

        public int? ReservationId { get; set; }
    }
}
