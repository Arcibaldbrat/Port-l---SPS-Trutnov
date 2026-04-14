using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class GradesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
