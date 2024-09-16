using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using Sporttiporssi.Configurations;

namespace Sporttiporssi.Services
{
    public class LoginService
    {
        private readonly LocalDatabaseService _databaseService;
        private readonly HttpClient _httpClient;

        public LoginService(LocalDatabaseService databaseService, HttpClient httpClient)
        {
            this._databaseService = databaseService;
            //var unsafeHttpClient = new UnsafeHttpClientHandler();
            //_httpClient = new HttpClient(unsafeHttpClient);
            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> RegisterUserAsync(string email, string password)
        {
            var payload = new
            {
                Email = email,
                Password = password
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ApiConfig.ApiBaseAddress}User", content);
            
            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Registration successful: " + responseBody);
                return response.StatusCode;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Registration failed: " + responseBody);
                return response.StatusCode;
            }
        }

        public async Task<bool> Login(string email, string password)
        {
            var request = new { email, password };
            var response = await _httpClient.PostAsJsonAsync($"{ApiConfig.ApiBaseAddress}User/login", request);

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                var token = result.Token;
                // Save token to securestorage
                await SecureStorage.SetAsync("auth_token", token);
                Preferences.Set("currentuser", email);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                // Log the request URL
                var requestUri = $"{ApiConfig.ApiBaseAddress}User/validate-token";
                Console.WriteLine($"Request URL: {requestUri}");

                // Make the request
                var response = await _httpClient.GetAsync(requestUri);

                // Log the response status code and content
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                // Return whether the response was successful
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
            //var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}User/validate-token");
            //return response.IsSuccessStatusCode;
        }

        public class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
