using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Port_SPS.Models;
using Port_SPS.Services;
using System.ComponentModel.DataAnnotations;

namespace Port_SPS.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailValidator _emailValidator;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailValidator emailValidator,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailValidator = emailValidator;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Jméno je povinné")]
            [Display(Name = "Jméno")]
            public string FirstName { get; set; } = "";

            [Required(ErrorMessage = "Příjmení je povinné")]
            [Display(Name = "Příjmení")]
            public string LastName { get; set; } = "";

            [Required(ErrorMessage = "Email je povinný")]
            [EmailAddress(ErrorMessage = "Neplatný email")]
            [Display(Name = "Školní email")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Heslo je povinné")]
            [StringLength(100, ErrorMessage = "Heslo musí být alespoň {2} znaků.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Heslo")]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Potvrzení hesla")]
            [Compare("Password", ErrorMessage = "Hesla se neshodují")]
            public string ConfirmPassword { get; set; } = "";

            [Required(ErrorMessage = "Musíte vybrat roli")]
            [Display(Name = "Jsem")]
            public string UserRole { get; set; } = "";
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Validace školního emailu
                if (!_emailValidator.IsValidSchoolEmail(Input.Email))
                {
                    ModelState.AddModelError("Input.Email", 
                        $"Musíte použít školní email (končící na {_emailValidator.GetEmailDomain()})");
                    return Page();
                }

                // Validace role
                if (Input.UserRole != "Student" && Input.UserRole != "Teacher")
                {
                    ModelState.AddModelError("Input.UserRole", "Neplatná role");
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Role = Input.UserRole,
                    IsApproved = Input.UserRole == "Student" // Studenti jsou auto-schváleni
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Uživatel vytvořil nový účet s emailem {Email}", Input.Email);

                    if (Input.UserRole == "Student")
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        // Učitel čeká na schválení
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
