using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class DeleteOrganisationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteOrganisationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Organisation Organisation { get; set; } = new Organisation();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Organisation = await _context.Organisations.FindAsync(id);

            if (Organisation == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Organisation = await _context.Organisations.FindAsync(id);

            if (Organisation != null)
            {
                _context.Organisations.Remove(Organisation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Success", new { message = "Organisation was deleted successfully!" });
        }
    }
}
