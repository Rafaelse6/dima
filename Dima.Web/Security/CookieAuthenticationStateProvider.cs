﻿using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Dima.Web.Security
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) :
        AuthenticationStateProvider,
        ICookieAuthenticationStateProvider
    {
        private bool _isAuthenticated = false;
        private readonly HttpClient _client = clientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAuthenticated;
        }

        public void NotifyAuthenticationStateChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            _isAuthenticated = false;
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var userInfo = await GetUser();
            if (userInfo is null)
               return new AuthenticationState(user);

            var claims = await GetClaims(userInfo); 

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
            user = new ClaimsPrincipal(id);

            _isAuthenticated = true;
            return new AuthenticationState(user);
        }

        private async Task<User?> GetUser()
        {
            try
            {
                return await _client.GetFromJsonAsync<User>("v1/identity/manage/info");

            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Claim>> GetClaims(User user)
        {

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.Email, user.Email),
            };

            return claims;
        }
    }
}
