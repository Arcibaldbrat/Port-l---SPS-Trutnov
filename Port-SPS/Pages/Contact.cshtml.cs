using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Port_SPS.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TempData["SuccessMessage"] = "Vaše zpráva byla úspěšně odeslána. Brzy vám odpovíme!";

            return RedirectToPage();
        }
    }
}
