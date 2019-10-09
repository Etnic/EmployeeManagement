using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        public readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    City = registerViewModel.City                   
                };

                var createUserResult = await userManager.CreateAsync(user, registerViewModel.Password);

                if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                {
                    return RedirectToAction("ListUsers", "Administration");
                }

                if (createUserResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "home");
                }

                foreach (var item in createUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View(registerViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var canSignIn = await this.signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

                if (canSignIn.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(loginViewModel);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailUsed(string email)
        {
            var result = await this.userManager.FindByEmailAsync(email);

            if (result == null)
            {
                Json(true);
            }
            else
            {
                return Json($"Email {email} was already taken");
            }

            return Json(true);
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.Url = returnUrl;
            return View();
        }
    }
}
