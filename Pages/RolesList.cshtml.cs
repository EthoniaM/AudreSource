using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class RolesListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RolesListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<RoleInfo> Roles { get; set; } = new List<RoleInfo>();

        public async Task OnGetAsync()
        {
            Roles = await _context.Roles.ToListAsync();
        }
    }
}

