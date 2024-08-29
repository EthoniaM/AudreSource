using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AudreSource.Pages
{
    public class KanBanDetailListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public KanBanDetailListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<KanBanDetail> KanBanDetails { get; set; }

        public async Task OnGetAsync()
        {
            KanBanDetails = await _context.KanBanDetails
                .Include(k => k.Kanban)
                .Include(k => k.User)
                .ToListAsync();
        }
    }
}
