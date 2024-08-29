using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class EditOrganisationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditOrganisationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public  Organisation Organisation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Organisation = await _context.Organisations.FindAsync(id);

            if (Organisation == null)
            {
                return NotFound();
            }
            return Page();
        }

        //checks if the model's data is valid after a form submission
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Sets the entity’s state to Modified, indicating that the entity has been changed and should be updated in the database when SaveChanges is called.
            _context.Attach(Organisation).State = EntityState.Modified;
            // Attempts to save changes to the database asynchronously. If successful, it updates the database with the latest changes.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganisationExists(Organisation.OrganisationID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex) // Catch any other exceptions
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the organisation: {ex.Message}");
                return Page();
            }

            return RedirectToPage("/Success", new { message = "Organisation updated successfully!" });
        }

        private bool OrganisationExists(int id)
        {
            return _context.Organisations.Any(e => e.OrganisationID == id);
        }
    }
}
