using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace AudreSource.Pages
{
    public class DeleteRoleModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteRoleModel(ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            RoleInfo = await _context.Roles.FindAsync(id);

            if (RoleInfo != null)
            {
                _context.Roles.Remove(RoleInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Success", new { message = "Roles was deleted successfully!" });
        }
    }
}

