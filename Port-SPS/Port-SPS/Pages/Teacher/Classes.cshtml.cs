using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages.Teacher
{
    [Authorize(Roles = "Teacher")]
    public class ClassesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
