namespace HMS_API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(20)]
        public string? UserRole { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }

}
