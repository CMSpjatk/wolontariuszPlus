using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly CMSDbContext _db;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            CMSDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [EmailAddress(ErrorMessage = "Wprowadzony email ma niepoprawny format.")]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [MaxLength(15, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
            [Display(Name = "Numer telefonu")]
            [Phone]
            public string PhoneNumber { get; set; }


            [Display(Name = "PESEL")]
            [StringLength(11)]
            public string PESEL { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
            [Display(Name = "Miasto")]
            public string City { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
            [Display(Name = "Ulica")]
            public string Street { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [Display(Name = "Numer budynku")]
            public int BuildingNumber { get; set; }

            
            [Display(Name = "Numer lokalu")]
            public int ApartmentNumber { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [RegularExpression("\\d{2}[-]\\d{3}", ErrorMessage = "Wprowadzony kod pocztowy ma niepoprawny format.")]
            [Display(Name = "Kod pocztowy")]
            public string PostalCode { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }


            [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła nie zgadzają się ze sobą.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    Address address = new Address(Input.City, Input.Street, Input.BuildingNumber, Input.ApartmentNumber, Input.PostalCode);

                    AppUser appUser = null;
                    if (!string.IsNullOrEmpty(Input.PESEL))
                    {
                        appUser = new Models.Volunteer
                        (
                            user.Id,
                            Input.FirstName,
                            Input.LastName,
                            Input.PhoneNumber,
                            address,
                            Input.PESEL
                        );
                    }
                    else
                    {
                        appUser = new Models.Organizer
                        (
                            user.Id,
                            Input.FirstName,
                            Input.LastName,
                            Input.PhoneNumber,
                            address
                        );
                    }

                    _db.AppUsers.Add(appUser);
                    _db.SaveChanges();

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
