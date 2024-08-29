using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class EditTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditTaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Kanban Task { get; set; }

        public async Task<IActionResult> OnGetAsync(int id) // Use "id" here to match the route
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Ensure StartDate and EndDate include the current time
                if (Task.StartDate.HasValue)
                {
                    // If StartDate is provided, include the current time
                    Task.StartDate = Task.StartDate.Value.Date + DateTime.Now.TimeOfDay;
                }
                else
                {
                    // If StartDate is null, set it to current date and time
                    Task.StartDate = DateTime.Now;
                }

                if (Task.EndDate.HasValue)
                {
                    // If EndDate is provided, include the current time
                    Task.EndDate = Task.EndDate.Value.Date + DateTime.Now.TimeOfDay;
                }
                else
                {
                    // If EndDate is null, set it to one week later with current time
                    Task.EndDate = DateTime.Now.AddDays(7);
                }

                _context.Attach(Task).State = EntityState.Modified;
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

            return RedirectToPage("/Success", new { message = "Task updated successfully!" });
        }

        private bool KanbanExists(int id)
        {
            return _context.Kanbans.Any(e => e.KanBanID == id);
        }
    }
}
