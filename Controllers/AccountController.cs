using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMvc.Models;
using AuthMvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signinmanager;

        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signinmanager)
        {
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
        }

        [HttpGet]
        // GET: /<controller>/
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await usermanager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(signinmanager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await signinmanager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("login", "account");
                }
                foreach( var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signinmanager.SignOutAsync();
            return RedirectToAction("login", "account");

        }


        [HttpGet]
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
                var result = await signinmanager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        //return LocalRedirect(returnUrl);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                    
                }
               
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
             

            }
            return View(model);
        }

    }
}
