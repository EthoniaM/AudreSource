using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class DeleteKanBanDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteKanBanDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public KanBanDetail? KanBanDetail { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (KanBanDetail == null)
            {
                return NotFound();
            }

            var kanbanDetail = await _context.KanBanDetails.FindAsync(KanBanDetail.KanBanDetailsID);

            if (kanbanDetail != null)
            {
                _context.KanBanDetails.Remove(kanbanDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/KanBanDetailList");
        }
    }
}

