using Domain.Entities;
using Domain.Models;
using Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.IService;
using System.Web;

namespace PlateformeDesJeunesAinSebaa.Controllers
{
#nullable disable
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IRecaptchaService _verificationService;
        private CaptchaSettings _captchaSettings;

        public AuthenticationController(IAuthenticationService authenticationService, UserManager<ApplicationUser> userManager,
            IEmailService emailService, IRecaptchaService verificationService, IOptions<CaptchaSettings> captchaSettings)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _emailService = emailService;
            _verificationService = verificationService;
            _captchaSettings = captchaSettings.Value;
        }

        [HttpGet]
        public IActionResult Login(string Success)
        {
            ViewData["ClientKey"] = _captchaSettings.ClientKey;
            if (Success == "1") ViewData["ClientPublicSucess"] = "1";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            try
            {
                // validate input
                var requestIsValid = await _verificationService.IsCaptchaValid(user.CaptchaResponse);
                if (requestIsValid)
                {
                    var userAuth = await _authenticationService.Login(user);
                    if (!userAuth.Success)
                    {
                        ViewData["ClientKey"] = _captchaSettings.ClientKey;
                        TempData["LoginError"] = userAuth.Message;
                        return View();
                    }

                    var roles = userAuth.Message.Split(',');

                    if (User.IsInRole("GestionneurBlog"))
                    {
                        return RedirectToAction("Index", "GestionContenu");
                    }
                    if (roles.Contains("GestionnaireFamille"))
                    {
                        return RedirectToAction("Index", "Famille");
                    }
                    return RedirectToAction("Index", "Candidats");
                }
                ViewData["ClientKey"] = _captchaSettings.ClientKey;
                TempData["LoginError"] = "Veuiller confirmer que vous n'êtes pas un robot.";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["ClientKey"] = _captchaSettings.ClientKey;
                TempData["LoginError"] = ex.Message.ToString();
                return View();
            }

        }


        [HttpGet]
        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["ClientKey"] = _captchaSettings.ClientKey;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            try
            {
                var requestIsValid = await _verificationService.IsCaptchaValid(user.CaptchaResponse);
                if (requestIsValid)
                {
                    var userAuth = await _authenticationService.Register(user);
                    if (!userAuth.Success)
                    {
                        TempData["LoginError"] = userAuth.Message;
                        ViewData["ClientKey"] = _captchaSettings.ClientKey;
                        return View();
                    }
                    //var user2 = await _userManager.FindByIdAsync(userAuth.Message2);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(
                    //   user2);
                    //string codeHtmlVersion = HttpUtility.UrlEncode(code);

                    //var link = Url.Action(nameof(VerifyEmail), "Authentication", new { userId = userAuth.Message2, code = codeHtmlVersion }, Request.Scheme, Request.Host.ToString());
                    //await _emailService.SendEmailAsyncConfirmEmail(user.Email, "Confirmation email", link);
                    return RedirectToAction("EmailConfirmation", "Authentication");
                }
                ViewData["ClientKey"] = _captchaSettings.ClientKey;
                TempData["LoginError"] = "Veuiller confirmer que vous n'êtes pas un robot.";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            else
            {
                var result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(code));
                if (result.Succeeded)
                {
                    return View();
                }
                else
                {
                    return BadRequest();
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return RedirectToAction("Login", "Authentication");
        }

        public IActionResult AccessDenied()
        {
            if (User.IsInRole("Finance"))
            {
                return RedirectToAction("Index", "Finances");
            }
            if (User.IsInRole("GestionneurBlog"))
            {
                return RedirectToAction("Index", "GestionContenu");
            }
            return View();
        }

    }
}
