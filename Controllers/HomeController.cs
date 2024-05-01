using BankFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.Requests;
using ProductApi.Models.Responses;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BankFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] public IActionResult Login() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var api = new HttpClient();

            var response = await api.PostAsJsonAsync("http://localhost:5209/api/login",request );
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                HttpContext.Session.SetString("Token", tokenResponse.Token);
                return Redirect("/Bank/Index");
            }

            //  Sess
            ModelState.AddModelError("Username", "Username or Password is wrong");
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
