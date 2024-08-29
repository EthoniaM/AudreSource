using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudreSource.Model
{
    public class UserDetail
    {
        [Key]
        public int UserDetailsID { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public string? Nationality { get; set; }
        public int UsersID { get; set; }

        // Navigation property
        [ForeignKey("UsersID")]
        public virtual User? User { get; set; }
    }
}
