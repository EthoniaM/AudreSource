using System.ComponentModel.DataAnnotations;

namespace AudreSource.Model
{
    public class Organisation
    {
        [Key]
        public int OrganisationID { get; set; }

        [Required(ErrorMessage = "OrganisationName is required.")]
        public string? OrganisationName { get; set; }

        [Required(ErrorMessage = "Representative is required.")]
        public string?Representative { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "ContactNumber is required.")]
        public string? ContactNumber { get; set; }
        [Required(ErrorMessage = "Domain is required.")]
        public string? Domain { get; set; }
    }
}
