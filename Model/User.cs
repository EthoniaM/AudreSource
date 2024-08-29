using System.ComponentModel.DataAnnotations;


namespace AudreSource.Model
{
    public class User
    {

        [Key]
        public int UsersID { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? EmailAddress { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$",
        ErrorMessage = "Password must include at least one special character and a number.")]

        public string? Password { get; set; }

        [Required(ErrorMessage = "First names are required.")]
        public string? FirstNames { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
      
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public string? Position { get; set; }
        public virtual UserDetail? UserDetails { get; set; }
    }
}
