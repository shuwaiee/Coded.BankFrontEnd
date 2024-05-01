using BankFrontEnd.API;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.Requests;
using ProductApi.Models.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankFrontEnd.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankAPIClient _api;
        public AccountController(BankAPIClient api)
        {
            _api = api;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignupRequest req)
        {
            var created = await _api.Register(req);
            if (created)
            {
                return Redirect("/Account/Login");
            }
            ModelState.AddModelError("Username", "Something happened, Could not create user");
            return View(req);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {

            var jwtToken = await _api.Login(request.Username, request.Password);
            if (jwtToken != "")
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

                var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);


                var authProperties = new AuthenticationProperties
                { IsPersistent = false, AllowRefresh = true };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                HttpContext.Session.SetString("Token", jwtToken);
                HttpContext.Response.Cookies.Append("Token", jwtToken);

                return Redirect("/Bank/Index");
            }


            ModelState.AddModelError("Username", "Invalid Username and/or Password");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Home/Index");
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
}
