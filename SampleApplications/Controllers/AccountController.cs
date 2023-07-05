using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleApplications.Database;
using SampleApplications.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace SampleApplications.Controllers
{
    public class AccountController : Controller
    {
        private readonly SampleApplicationDBContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(SampleApplicationDBContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult LoginAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "User not valid");
                    return View(loginModel);
                }
                var result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
                if (result)
                {
                    var claims =new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, loginModel.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    var identity = new ClaimsIdentity(
                         CookieAuthenticationDefaults.AuthenticationScheme
                       , ClaimTypes.Name
                       , ClaimTypes.Role);
                    identity.AddClaims(claims);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                        IsPersistent = true,
                        IssuedUtc = DateTimeOffset.UtcNow,
                    };
                    await HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme
                          , principal
                          , properties);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        UserName = model.Email,

                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded!=true)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);
                        }
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                return View();
            }
            catch
            {
                throw;
            }
        }
    }
}
