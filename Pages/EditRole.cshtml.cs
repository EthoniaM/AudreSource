using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AudreSource.Pages
{
    public class EditRoleModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditRoleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoleInfo RoleInfo { get; set; } = new RoleInfo();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            RoleInfo = await _context.Roles.FindAsync(id);

            if (RoleInfo == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RoleInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(RoleInfo.RoleID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Success", new { message = "Roles was updated successfully!" });
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleID == id);
        }
    }
}