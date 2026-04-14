using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Port_SPS.Models;
using System.ComponentModel.DataAnnotations;

namespace Port_SPS.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email je povinný")]
            [EmailAddress(ErrorMessage = "Neplatný email")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Heslo je povinné")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Display(Name = "Pamatuj si mě")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                
                if (user != null && !user.IsApproved)
                {
                    ModelState.AddModelError(string.Empty, "Váš účet čeká na schválení správce školy.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Uživatel se přihlásil.");
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Uživatelský účet je uzamčen.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
