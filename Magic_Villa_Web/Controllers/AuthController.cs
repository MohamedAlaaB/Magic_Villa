using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Magic_Villa_Web.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Login()
        {
            var loginobj = new LoginRequestDto();
            return View(loginobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginobj)
        {
            var data = await _authService.LoginAsync<APIResponse>(loginobj);
            if (data != null && data.IsSuccess == true)
            {
                var loginres = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(data.Result));
                HttpContext.Session.SetString(SD.session, loginres.Token);
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginres.Token);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));
                var princibal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princibal);
                return RedirectToAction("Index", "Home");
            }
           
            return View(loginobj);
        }
        public async Task<IActionResult> Register()
        {
            var regobj = new RegisterationRequest() ;
            return View(regobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequest regobj)
        {
            regobj.Role = "customer";
            var data = await _authService.RegisterAsync<APIResponse>(regobj);
            if (data != null && data.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View(regobj);
        }
        public async Task<IActionResult> AccessDined()
        {
           
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
