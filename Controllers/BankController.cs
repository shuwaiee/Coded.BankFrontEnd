using BankFrontEnd.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using System.Net.Http.Headers;

namespace BankFrontEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BankController : Controller
    {
        private readonly BankAPIClient _client;
        public BankController(BankAPIClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            var banks = await _client.GetBanks();
            return View(banks.Data);
        }
    }
}
