using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication.Shared.User;

namespace WebApplication.Shared.Services
{
    public class AuthenticationService
    {
        private readonly string _baseUrl;
        
        public AuthenticationService(string url)
        {
            _baseUrl = url;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            HttpClient client = new HttpClient();

            var jsonData = JsonConvert.SerializeObject(model);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_baseUrl}/api/auth/register", content);

            var responsBody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responsBody);

            return responseObject;
        }
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            HttpClient client = new HttpClient();

            var jsonData = JsonConvert.SerializeObject(model);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_baseUrl}/api/auth/login", content);

            var responsBody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responsBody);

            return responseObject;
        }
    }
}
