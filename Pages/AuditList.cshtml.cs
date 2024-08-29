using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class AuditListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuditListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Audit> Audits { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch all audits from the database
            Audits = await _context.Audits.ToListAsync();
        }
    }
}
