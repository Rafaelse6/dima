using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using System.Net.Http.Json;

namespace Dima.Web.Handlers
{
    public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<Response<string>> LoginAsync(LoginRequest request)
        {
            await _client.PostAsJsonAsync("v1/identity/login");
        }

        public Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }       
    }
}
