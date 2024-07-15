using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS_API.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CreditCardDetails { get; set; }
        public int? BillId { get; set; }

        [ForeignKey(nameof(BillId))]
        public Bill? Bill { get; set; }
    }
}
