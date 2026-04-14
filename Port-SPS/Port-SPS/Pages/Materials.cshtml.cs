using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages
{
    [Authorize]
    public class MaterialsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
