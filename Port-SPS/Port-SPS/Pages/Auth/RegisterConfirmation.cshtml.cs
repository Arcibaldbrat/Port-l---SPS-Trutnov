using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages.Auth
{
    public class RegisterConfirmationModel : PageModel
    {
        public string? Email { get; set; }

        public void OnGet(string? email = null)
        {
            Email = email;
        }
    }
}
