using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class DeleteTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteTaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Kanban Task { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _context.Kanbans.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Task == null)
            {
                return NotFound();
            }

            try
            {
                _context.Kanbans.Remove(Task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KanbanExists(Task.KanBanID))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("/Success", new { message = "Task deleted successfully!" });
        }

        private bool KanbanExists(int id)
        {
            return _context.Kanbans.Any(e => e.KanBanID == id);
        }
    }
}
