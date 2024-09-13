using Sporttiporssi.Configurations;
using Sporttiporssi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml;
using Microsoft.Maui.ApplicationModel.Communication;
using Sporttiporssi.Models.DTOs;

namespace Sporttiporssi.Services
{
    public class GroupService
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<FantasyGroup> Groups { get; set; } = new ObservableCollection<FantasyGroup>();
        public GroupService(HttpClient httpClient)
        {
            //_httpClient = httpClient;
            var unsafeHttpClient = new UnsafeHttpClientHandler();
            _httpClient = new HttpClient(unsafeHttpClient);
        }

        public async Task<List<FantasyGroup>> AllFantasyGroupsBySerie()
        {
            var serie = Preferences.Get("currentserie", string.Empty);
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}group?serie={serie}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FantasyGroup>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching groups: {ex.Message}");
                return new List<FantasyGroup>();
            }
        }

        public async Task<bool> SaveNewGroup(GroupRegisterDto dto)
        {
            var serie = Preferences.Get("currentserie", string.Empty);
            var userEmail = Preferences.Get("currentuser", string.Empty);
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                dto.Serie = serie;               
                var jsonPayload = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                Console.WriteLine("Payload is: " + jsonPayload);
                var response = await _httpClient.PostAsync($"{ApiConfig.ApiBaseAddress}group", content);
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
            catch(Exception ex) 
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<GroupDataResult>> GetGroupDataAndStandingByTeamId(Guid teamId)
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}group/GetGroupDataAndStandingByTeamId?teamId={teamId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GroupDataResult>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching groups: {ex.Message}");
                return new List<GroupDataResult>();
            }
        }

        public async Task<bool> JoinGroup(Guid groupId, Guid teamId, string password)
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var payload = new 
                {
                    Email = "",
                    Password = password,                   
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{ApiConfig.ApiBaseAddress}group/JoinGroup?groupId={groupId}&teamId={teamId}", content);
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

        public async Task<bool> LeaveGroup(string groupName, Guid teamId)
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            string serie = Preferences.Get("currentserie", string.Empty);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {             
                var response = await _httpClient.DeleteAsync($"{ApiConfig.ApiBaseAddress}group/LeaveGroup?groupName={groupName}&serie={serie}&teamId={teamId}");
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
