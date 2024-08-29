using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudreSource.Pages
{
    public class SuccessModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet(string message)
        {
            Message = message;
        }

    }

}
