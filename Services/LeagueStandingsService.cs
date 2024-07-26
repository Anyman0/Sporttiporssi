using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Sporttiporssi.Configurations;
using System.Net.Http.Headers;

namespace Sporttiporssi.Services
{  
    public class LeagueStandingsService
    {
        private readonly LocalDatabaseService _databaseService;
        private readonly HttpClient _httpClient;
        public ObservableCollection<LeagueStanding> LeagueStandings { get; set; } = new ObservableCollection<LeagueStanding>();
        public LeagueStandingsService(LocalDatabaseService databaseService, HttpClient httpClient)
        {
            _databaseService = databaseService;
            _httpClient = httpClient;
        }

        public async Task<List<LeagueStanding>> GetStandingsAsync()
        {
            bool needToUpdate = false;
            try
            {
                var localStandings = await _databaseService.GetAllStandingsAsync();
                if(LeagueStandings.Count > 0)
                {
                    // Update seasonstandings every 3 hour
                    needToUpdate = localStandings.Any(s => s.LastUpdated < DateTime.UtcNow.AddHours(-3));
                }
                else
                {
                    needToUpdate = true;
                }
                if(localStandings.Count == 0 || needToUpdate)
                {
                    await LoadStandingsAsync();
                    if(LeagueStandings != null)
                    {
                        foreach (var standing in LeagueStandings)
                        {
                            var existingStanding = await _databaseService.GetStandingAsync(standing.TeamId);
                            if (existingStanding != null)
                            {
                                standing.LastUpdated = DateTime.UtcNow;
                                await _databaseService.UpdateStandingAsync(standing);
                            }
                            else
                            {
                                standing.LastUpdated = DateTime.UtcNow;
                                await _databaseService.SaveSeasonAsync(standing);
                            }
                        }
                        return LeagueStandings.ToList();
                    }
                }
                return localStandings.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return new List<LeagueStanding>();
            }
        }

        private async Task LoadStandingsAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {               
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}liigastanding");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var standings = JsonConvert.DeserializeObject<List<LeagueStanding>>(json);
                LeagueStandings.Clear();
                foreach (var team in standings)
                {
                    var standing = new LeagueStanding
                    {
                        TeamId = team.TeamId,
                        TeamName = team.TeamName,
                        Ranking = team.Ranking,
                        Games = team.Games,
                        Wins = team.Wins,
                        Ties = team.Ties,
                        Losses = team.Losses,
                        Goals = team.Goals,
                        GoalsAgainst = team.GoalsAgainst,
                        GoalDifference = team.Goals - team.GoalsAgainst,
                        Points = team.Points,
                        PointsPerGame = team.PointsPerGame,
                        LastUpdated = team.LastUpdated,
                    };
                    LeagueStandings.Add(standing);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
            }
        }
    }
}
