using AudreSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AudreySource.Data;

namespace AudreSource.Pages
{
    public class CreateUserDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateUserDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserDetail UserDetail { get; set; }

        // List of users for dropdown
        public IEnumerable<SelectListItem> UsersSelectList { get; set; } = Enumerable.Empty<SelectListItem>();

        public async Task OnGetAsync()
        {
            // Fetch users from the database
            var users = await _context.Users.ToListAsync();

            // Populate the select list
            UsersSelectList = users.Select(u => new SelectListItem
            {
                Value = u.UsersID.ToString(),
                Text = $"{u.FirstNames} {u.Surname}" // Display name can be adjusted as needed
            });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate the UsersSelectList in case of validation errors
                var users = await _context.Users.ToListAsync();
                UsersSelectList = users.Select(u => new SelectListItem
                {
                    Value = u.UsersID.ToString(),
                    Text = $"{u.FirstNames} {u.Surname}"
                });
                return Page();
            }

            _context.UserDetails.Add(UserDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Success", new { message = "UserDetails created successfully!" });
        }
    }
}
