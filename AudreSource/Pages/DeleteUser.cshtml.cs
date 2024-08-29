using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class DeleteUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null)
            {
                return NotFound();
            }

            _context.Users.Remove(User);

            try
            {
                await _context.SaveChangesAsync();
                // Redirect to Success page with a message
                return RedirectToPage("/UsersList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the user: {ex.Message}");
                return Page();
            }
        }
    }
}
