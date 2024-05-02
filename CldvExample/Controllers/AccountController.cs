﻿using KhumaloeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KhumaloeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ProductUser> _userManager;
        private readonly SignInManager<ProductUser> _signInManager;

        public AccountController(UserManager<ProductUser> userManager, SignInManager<ProductUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel.InputModel(); // Initialize the InputModel
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel.InputModel model, bool isAdmin = false)
        {
            if (ModelState.IsValid)
            {
                var user = new ProductUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign admin role if specified during registration
                    if (isAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay the form with validation errors
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel.InputModel(); // Initialize the InputModel
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel.InputModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        // Redirect admin user to admin area
                        return RedirectToAction("AdminHome", "Admin");
                    }
                    // Redirect regular user to default area
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we got this far, something failed, redisplay the form with validation errors
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

}
