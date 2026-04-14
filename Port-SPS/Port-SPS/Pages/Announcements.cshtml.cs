using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages
{
    [Authorize]
    public class AnnouncementsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
