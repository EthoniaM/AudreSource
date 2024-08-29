using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudreSource.Pages
{
    public class DeleteUserDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserDetail UserDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserDetail = await _context.UserDetails.FindAsync(id);

            if (UserDetail == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            UserDetail = await _context.UserDetails.FindAsync(id);

            if (UserDetail != null)
            {
                _context.UserDetails.Remove(UserDetail);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Success", new { message = "UserDetails was Deleted successfully!" });
        }
    }

}

