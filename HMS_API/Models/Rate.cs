using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS_API.Models
{
    public class Rate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RateId { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Day { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PerNightCharges { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCharges { get; set; }

    }
}
