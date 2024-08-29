using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudreSource.Pages
{
    public class EditUserDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditUserDetailModel(ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!_context.UserDetails.Any(e => e.UserDetailsID == UserDetail.UserDetailsID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Success", new { message = "UserDetails updated successfully!" });
        }
    }
}

