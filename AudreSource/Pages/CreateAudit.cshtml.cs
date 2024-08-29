using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class CreateAuditModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public CreateAuditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Audit Audit { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Log or debug to see why the model is not valid
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            // Log to verify the method is executing
            Console.WriteLine("OnPostAsync is triggered");

            Audit.AuditNumber = await GenerateUniqueAuditNumberAsync();
            Audit.CreatedOn = DateTime.Now;

            _context.Audits.Add(Audit);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Success", new { message = "Audit created successfully!" });
        }

        private async Task<int> GenerateUniqueAuditNumberAsync()
        {
            // Fetch the current maximum AuditNumber in the database
            int maxAuditNumber = await _context.Audits.MaxAsync(a => (int?)a.AuditNumber) ?? 0;

            // Return the next available AuditNumber
            return maxAuditNumber + 1;
        }
    }
}