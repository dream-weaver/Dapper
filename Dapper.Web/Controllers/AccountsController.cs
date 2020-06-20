using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper.Data;
using Dapper.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dapper.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<AppIdentityStoreUser> _userManager;
        private readonly SignInManager<AppIdentityStoreUser> signInManager;
        private readonly IConfiguration config;

        public AccountsController(UserManager<AppIdentityStoreUser> userManager, 
                                SignInManager<AppIdentityStoreUser> signInManager,
                                IConfiguration config)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(registerModel.UserName);
                if (user == null)
                {
                    user = new AppIdentityStoreUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        Email = registerModel.Email,
                        UserName = registerModel.UserName,
                        NormalizedUserName = registerModel.UserName
                    };
                    var result = await _userManager.CreateAsync(user, registerModel.Password);
                }
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var signinresult = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
                if (signinresult.Succeeded)
                {
                    TempData["LoggedInUser"] = loginModel.UserName;
                    return RedirectToAction("Index", "Contacts");
                }
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user!=null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            config["Tokens:Issuer"],
                            config["Tokens:Audience"],
                            claims, 
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: creds
                        );
                        var results = new {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo 
                        };
                        return Created("", results);
                    }
                }
            } 
            return BadRequest();
        }
    }
   
}