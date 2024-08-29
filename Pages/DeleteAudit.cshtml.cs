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

        public async Task<IActionResult> OnPostAsync()
        {
            if (Audit == null || !ModelState.IsValid)
            {
                return Page();
            }

            var auditToDelete = await _context.Audits.FindAsync(Audit.AuditID);
            if (auditToDelete == null)
            {
                return NotFound();
            }

            _context.Audits.Remove(auditToDelete);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Success", new { message = "Audit was deleted successfully!" });
        }
    }
}
