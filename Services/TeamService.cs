using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Models;
using System.Net.Http.Headers;
using Sporttiporssi.Configurations;
using Newtonsoft.Json;
using System.Diagnostics;
using Sporttiporssi.Models.DTOs;

namespace Sporttiporssi.Services
{
    public class TeamService
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<FantasyTeam> Teams = new ObservableCollection<FantasyTeam>();

        public TeamService(HttpClient httpclient)
        {
            //_httpClient = httpClient;
            var unsafeHttpClient = new UnsafeHttpClientHandler();
            _httpClient = new HttpClient(unsafeHttpClient);
        }

        public async Task<List<FantasyTeam>> AllFantasyTeamsBySerie()
        {
            var serie = Preferences.Get("currentserie", string.Empty);           
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}fantasyteam?serie={serie}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FantasyTeam>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching teams: {ex.Message}");
                return new List<FantasyTeam>();
            }
        }

        public async Task<HockeyDefaultFTP> GetHockeyDefaultFTPBySerie()
        {
            var serie = Preferences.Get("currentserie", string.Empty);
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}fantasyteam/HockeyDefault?serie={serie}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<HockeyDefaultFTP>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching teams: {ex.Message}");
                return new HockeyDefaultFTP();
            }
        }

        public async Task<ObservableCollection<FantasyTeam>> AllFantasyTeamsByUserAndSerie()
        {
            var serie = Preferences.Get("currentserie", string.Empty);
            var user = Preferences.Get("currentuser", string.Empty);
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}fantasyteam/AllByUser?serie={serie}&email={user}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<FantasyTeam>>(json);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error fetching teams: {ex.Message}");
                return new ObservableCollection<FantasyTeam>();
            }
        }

        public async Task<bool> CanTrade()
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}fantasyteam/CanTrade");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SaveNewTeam(TeamRegisterDto dto)
        {
            var serie = Preferences.Get("currentserie", string.Empty);
            var userEmail = Preferences.Get("currentuser", string.Empty);
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {               
                var payload = new TeamRegisterDto
                {
                    Email = userEmail,
                    Serie = serie,
                    TeamName = dto.TeamName,
                    TradesPerPhase = dto.TradesPerPhase,
                    FundsLeft = dto.FundsLeft,
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                Console.WriteLine(jsonPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{ApiConfig.ApiBaseAddress}fantasyteam", content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}
