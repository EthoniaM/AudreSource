using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudreSource.Pages
{
    public class DeleteAuditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteAuditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Audit Audit { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Audit = await _context.Audits.FindAsync(id);

            if (Audit == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var auditToDelete = await _context.Audits.FindAsync(id);
            if (auditToDelete == null)
            {
                return NotFound();
            }

            _context.Audits.Remove(auditToDelete);
            await _context.SaveChangesAsync();

            // Redirect to a valid page after deletion
            return RedirectToPage("/AuditList");
        }
    }
}
