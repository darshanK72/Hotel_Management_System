using System.ComponentModel.DataAnnotations;

namespace HMS_API.Payload
{
    public class UserPayload
    {
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}
