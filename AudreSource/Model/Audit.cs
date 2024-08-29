using System.ComponentModel.DataAnnotations;

namespace AudreSource.Model
{
    public class Audit
    {
        [Key]
        public int AuditID { get; set; }

        [Required]
        [StringLength(100)]
        public string? AuditTitle { get; set; }

        public int? AuditNumber { get; set; }

        public string? AuditDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Kanban> Kanbans { get; set; } = new List<Kanban>();
    }
}
