using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS_API.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(10)]
        public string? RoomNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PerNightCharges { get; set; }

        [Required]
        [EnumDataType(typeof(RoomType))]
        public RoomType RoomType { get; set; }
    }
}
