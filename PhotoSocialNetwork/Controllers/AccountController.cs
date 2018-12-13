using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels.Account;

namespace PhotoSocialNetwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStorage _storage;

        public AccountController(IStorage storage)
        {
            _storage = storage;
        }

        public IActionResult Login()
        {
            return View("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                if (_storage.IsUserAutenticationInfoCorrect(model.Login, model.Password))
                {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View("LogIn");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_storage.UserExists(model.Login, model.Email))
                {
                    _storage.AddUser(model);

                    await Authenticate(model.Login);

                    return RedirectToAction("Login", "Account");
                }
                else
                    ModelState.AddModelError("", "The user with such login or email already exists");
            }

            return View(model);
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, login) };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
