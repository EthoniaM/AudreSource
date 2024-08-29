using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class EditKanBanDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditKanBanDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public KanBanDetail KanBanDetail { get; set; }

        public IEnumerable<SelectListItem> UsersSelectList { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> KanBansSelectList { get; set; } = Enumerable.Empty<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            KanBanDetail = await _context.KanBanDetails
                .Include(k => k.Kanban)
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.KanBanDetailsID == id);

            if (KanBanDetail == null)
            {
                return NotFound();
            }

            // Fetch users from the database
            UsersSelectList = await _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.UsersID.ToString(),
                    Text = $"{u.FirstNames} {u.Surname}"
                }).ToListAsync();

            // Fetch KanBans from the database
            KanBansSelectList = await _context.Kanbans
                .Select(k => new SelectListItem
                {
                    Value = k.KanBanID.ToString(),
                    Text = k.TaskTitle
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (KanBanDetail == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Repopulate select lists in case of validation errors
                UsersSelectList = await _context.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.UsersID.ToString(),
                        Text = $"{u.FirstNames} {u.Surname}"
                    }).ToListAsync();

                KanBansSelectList = await _context.Kanbans
                    .Select(k => new SelectListItem
                    {
                        Value = k.KanBanID.ToString(),
                        Text = k.TaskTitle
                    }).ToListAsync();

                return Page();
            }

            _context.Attach(KanBanDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Success", new { message = "KanBanDetail created successfully!" });
        }
    }
}