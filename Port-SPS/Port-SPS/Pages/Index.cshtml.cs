using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Port_SPS.Models;

namespace Port_SPS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser? CurrentUser { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                CurrentUser = await _userManager.GetUserAsync(User);
            }
        }
    }
}
