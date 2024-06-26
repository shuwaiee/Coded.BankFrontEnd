﻿using System.Net.Http.Headers;
using System.Net.Http;
using ProductApi.Models;
using ProductApi.Models.Responses;
using Microsoft.AspNetCore.Identity.Data;
using ProductApi.Models.Requests;

namespace BankFrontEnd.API
{
    public class BankAPIClient
    {
        private readonly HttpClient _api;
        public BankAPIClient(IHttpContextAccessor accessor, IHttpClientFactory factory)
        {
            _api = factory.CreateClient("bankApi");

            var token = accessor.HttpContext.Session.GetString("Token");
            _api.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<PageListResult<BankBranchResponse>> GetBanks()
        {
            var response = await _api
                .GetFromJsonAsync<PageListResult<BankBranchResponse>>("api/bank");
            return response;
        }
        public async Task<bool> Register(SignupRequest request)
        {
            var response = await _api.PostAsJsonAsync("/api/login/Registor", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<string> Login(string username, string password)
        {
            var response = await _api.PostAsJsonAsync("/api/login",
                new UserLoginRequest { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

                var token = tokenResponse.Token;
                return token;
            }
            return "";
        }
    }
}
