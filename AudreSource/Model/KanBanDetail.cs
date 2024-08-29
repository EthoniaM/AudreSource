using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudreSource.Model
{
    public class KanBanDetail
    {
        [Key]
        public int KanBanDetailsID { get; set; }

        public int KanBanID { get; set; }
        public int UsersID { get; set; }

        public string? Comments { get; set; }

        // Navigation properties
        [ForeignKey("KanBanID")]
        public Kanban? Kanban { get; set; }

        [ForeignKey("UsersID")]
        public User? User { get; set; }
    }
}
