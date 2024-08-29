using System.ComponentModel.DataAnnotations;

namespace AudreSource.Model
{
    public class RoleInfo
    {
        [Key]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Role is required.")]

        public string? Role { get; set; }

        [Required(ErrorMessage = "RoleDescription required.")]

        public string? RoleDescription { get; set; } 
    }
}
