using AudreySource.Data;
using AudreSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class CreateTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateTaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Kanban NewTask { get; set; }

        public IList<Audit> Audits { get; set; } // List of existing audits to populate the dropdown

        public int AuditID { get; set; }

        public async Task<IActionResult> OnGetAsync(int? auditId)
        {
            if (auditId == null)
            {
                return NotFound();
            }

            AuditID = auditId.Value;

            // Initialize NewTask with current date and time
            NewTask = new Kanban
            {
                AuditID = AuditID,
                StartDate = DateTime.Now, // Include current date and time
                EndDate = DateTime.Now.AddDays(7) // Default to one week later with current time
            };

            // Retrieve all existing audits for the dropdown list
            Audits = await _context.Audits.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate audits if the form is invalid
                Audits = await _context.Audits.ToListAsync();
                return Page();
            }

            try
            {
                // Ensure StartDate and EndDate include the current time
                if (NewTask.StartDate.HasValue)
                {
                    // If StartDate is provided, include the current time
                    NewTask.StartDate = NewTask.StartDate.Value.Date + DateTime.Now.TimeOfDay;
                }
                else
                {
                    // If StartDate is null, set it to current date and time
                    NewTask.StartDate = DateTime.Now;
                }

                if (NewTask.EndDate.HasValue)
                {
                    // If EndDate is provided, include the current time
                    NewTask.EndDate = NewTask.EndDate.Value.Date + DateTime.Now.TimeOfDay;
                }
                else
                {
                    // If EndDate is null, set it to one week later with current time
                    NewTask.EndDate = DateTime.Now.AddDays(7);
                }

                _context.Kanbans.Add(NewTask);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions and show error messages
                ModelState.AddModelError(string.Empty, $"Error saving task: {ex.Message}");
                Audits = await _context.Audits.ToListAsync(); // Repopulate audits in case of error
                return Page();
            }

            return RedirectToPage("/Success", new { message = "Task created successfully!" });
        }
    }
}
