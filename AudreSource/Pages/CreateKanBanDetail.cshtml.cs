using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class CreateKanBanDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateKanBanDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public KanBanDetail KanBanDetail { get; set; }

        // List of users for dropdown
        public IEnumerable<SelectListItem> UsersSelectList { get; set; } = Enumerable.Empty<SelectListItem>();

        // List of KanBans for dropdown
        public IEnumerable<SelectListItem> KanBansSelectList { get; set; } = Enumerable.Empty<SelectListItem>();

        public async Task OnGetAsync()
        {
            // Fetch users from the database
            var users = await _context.Users.ToListAsync();

            // Populate the select list for users
            UsersSelectList = users.Select(u => new SelectListItem
            {
                Value = u.UsersID.ToString(),
                Text = $"{u.FirstNames} {u.Surname}" // Display name can be adjusted as needed
            });

            // Fetch KanBans from the database
            var kanbans = await _context.Kanbans.ToListAsync();

            // Populate the select list for KanBans
            KanBansSelectList = kanbans.Select(k => new SelectListItem
            {
                Value = k.KanBanID.ToString(),
                Text = $"{k.TaskTitle}" // Display name can be adjusted as needed
            });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate the UsersSelectList and KanBansSelectList in case of validation errors
                var users = await _context.Users.ToListAsync();
                UsersSelectList = users.Select(u => new SelectListItem
                {
                    Value = u.UsersID.ToString(),
                    Text = $"{u.FirstNames} {u.Surname}"
                });

                var kanbans = await _context.Kanbans.ToListAsync();
                KanBansSelectList = kanbans.Select(k => new SelectListItem
                {
                    Value = k.KanBanID.ToString(),
                    Text = $"{k.TaskTitle}"
                });

                return Page();
            }

            _context.KanBanDetails.Add(KanBanDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Success", new { message = "KanBanDetail created successfully!" });
        }
    }
}
