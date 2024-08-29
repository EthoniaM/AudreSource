using AudreSource.Model;
using AudreySource.Data;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        //Attempts to delete the tasks from the kanbans Table in the database and if something goes wrong while doing then the catch block handles the error.
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

            return RedirectToPage("/KanbanBoard");
        }
        //checks if a Kanban record with a specific id exists in the database.
        //used to verify the existence of a record before performing operations like updates or deletions.
        private bool KanbanExists(int id)
        {
            return _context.Kanbans.Any(e => e.KanBanID == id);
        }
    }
}
