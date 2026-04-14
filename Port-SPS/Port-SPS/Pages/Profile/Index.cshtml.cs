using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Port_SPS.Models;

namespace Port_SPS.Pages.Profile
{
    [Authorize]
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
            CurrentUser = await _userManager.GetUserAsync(User);
        }
    }
}
