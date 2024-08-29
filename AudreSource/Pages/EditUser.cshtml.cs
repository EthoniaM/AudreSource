using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class EditUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditUserModel(ApplicationDbContext context)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.UsersID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex) // Catch any other exceptions
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the user: {ex.Message}");
                return Page();
            }

            return RedirectToPage("/Success", new { message = "User updated successfully!" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UsersID == id);
        }
    }
}
