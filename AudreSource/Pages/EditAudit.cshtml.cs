using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class EditAuditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditAuditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Audit Audit { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the Audit entity from the database
            Audit = await _context.Audits.FindAsync(id);

            if (Audit == null)
            {
                return NotFound();
            }

            // Ensure CreatedOn is set to a valid date if it's not initialized
            if (Audit.CreatedOn == DateTime.MinValue)
            {
                Audit.CreatedOn = DateTime.Now;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure the entity is tracked correctly
            var auditToUpdate = await _context.Audits.FindAsync(Audit.AuditID);

            if (auditToUpdate == null)
            {
                return NotFound();
            }

            // Update the properties
            auditToUpdate.AuditTitle = Audit.AuditTitle;
            auditToUpdate.AuditDescription = Audit.AuditDescription;

            // Ensure CreatedOn is set to a valid date
            auditToUpdate.CreatedOn = Audit.CreatedOn != DateTime.MinValue ? Audit.CreatedOn : DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditExists(Audit.AuditID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Success", new { message = "Audit edited successfully!" });

        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.AuditID == id);
        }
    }
}
