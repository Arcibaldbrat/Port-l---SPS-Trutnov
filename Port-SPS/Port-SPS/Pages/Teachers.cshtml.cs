using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages
{
    [Authorize]
    public class TeachersModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
