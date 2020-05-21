using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Leave_Management.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Leave_Management.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Employee> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<Employee> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} وارد شده می بایست حداقل شامل {2} حرف و حداکثر شامل {1} حرف باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "رمز عبور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تائید رمز عبور")]
            [Compare("Password", ErrorMessage = "رمز عبور وارد شده و تکرار آن با هم مطابقت ندارد.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "ورود {0} کاربر الزامی می باشد.")]
            [Display(Name = "نام")]
            [DataType(DataType.Text)]
            public string Firstname { get; set; }

            [Required(ErrorMessage = "ورود {0} کاربر الزامی می باشد.")]
            [Display(Name = "نام خانوادگی")]
            [DataType(DataType.Text)]
            public string Lastname { get; set; }

            [Required(ErrorMessage = "ورود {0} کاربر الزامی می باشد.")]
            [Display(Name = "تلفن همراه")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) 
                return Page();

            var user = new Employee
            {
                UserName = Input.Email,
                Email = Input.Email,
                Firstname = Input.Firstname,
                Lastname = Input.Lastname,
                PhoneNumber = Input.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var adminUserExists = await _userManager.FindByNameAsync("Administrator").ConfigureAwait(false);
                var employeeRoleExists = await _roleManager.FindByNameAsync("Employee").ConfigureAwait(false);
                if (adminUserExists != null && employeeRoleExists!=null)
                    await _userManager.AddToRoleAsync(user, "Employee");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                await SendConfimEmail(callbackUrl);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SendConfimEmail(string callbackUrl)
        {
            await _emailSender.SendEmailAsync(Input.Email, "تائید ایمیل",
                $"لطفا ایمیل خود را با کلیک بر روی <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>این لینک</a> تائید فرمائید.");
        }
    }
}
