using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages
{
    [Authorize]
    public class MessagesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
