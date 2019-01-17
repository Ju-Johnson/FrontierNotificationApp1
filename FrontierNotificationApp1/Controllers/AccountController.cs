using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontierNotificationApp1.Models;
using FrontierNotificationApp1.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FrontierNotificationApp1.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(Login model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					
					return RedirectToAction(nameof(IncidentsController.Index), "Incidents");
				}
				
				if (result.IsLockedOut)
				{
					
					return View("Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// POST: /Account/LogOut
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(Register model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { FirstName = model.FirstName, LastName = model.LastName, UserId = model.UserId, UserName = model.Email, Email = model.Email, Department = model.Department };
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					
					await _signInManager.SignInAsync(user, isPersistent: false);
					
					return RedirectToAction(nameof(IncidentsController.Index), "Incidents");
				}
				else
				{
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}
	}
}
