using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Port_SPS.Models;
using System.ComponentModel.DataAnnotations;

namespace Port_SPS.Pages.Profile
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required(ErrorMessage = "Staré heslo je povinné")]
            [DataType(DataType.Password)]
            [Display(Name = "Staré heslo")]
            public string OldPassword { get; set; } = "";

            [Required(ErrorMessage = "Nové heslo je povinné")]
            [StringLength(100, ErrorMessage = "Heslo musí být alespoň {2} znaků.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Nové heslo")]
            public string NewPassword { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Potvrzení nového hesla")]
            [Compare("NewPassword", ErrorMessage = "Hesla se neshodují")]
            public string ConfirmPassword { get; set; } = "";
        }

        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("Uživatel úspěšně změnil heslo.");
            TempData["SuccessMessage"] = "Vaše heslo bylo úspěšně změněno.";
            return RedirectToPage("./Index");
        }
    }
}
