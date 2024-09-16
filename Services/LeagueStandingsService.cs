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
            //var unsafeHttpClient = new UnsafeHttpClientHandler();
            //_httpClient = new HttpClient(unsafeHttpClient);
        }
        public async Task<ObservableCollection<LeagueStanding>> GetLeagueStandings()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var league = Preferences.Get("currentserie", string.Empty);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}liigastanding?league={league}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var standings = JsonConvert.DeserializeObject<List<LeagueStanding>>(json);
                LeagueStandings.Clear();
                foreach (var team in standings)
                {
                    if (team.TeamName == "Kaerpaet" || team.TeamName == "Aessaet" || team.TeamName == "Kiekko-Espoo")
                    {
                        team.TeamName = TeamNameConverter(team.TeamName);
                    }
                    var standing = new LeagueStanding
                    {
                        Rank = team.Rank,                       
                        TeamName = team.TeamName,
                        Points = team.Points,
                        Played = team.Played,
                        Wins = team.Wins,
                        Losses = team.Losses,
                        GoalsFor = team.GoalsFor,
                        GoalsAgainst = team.GoalsAgainst,
                        GoalDifference = team.GoalDifference,                        
                        LastUpdated = team.LastUpdated,
                    };
                    LeagueStandings.Add(standing);
                }
                return LeagueStandings;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return null;
            }
        }

        private string TeamNameConverter(string teamName)
        {
            if (teamName == "Aessaet") return "Ässät";
            if (teamName == "Kaerpaet") return "Kärpät";
            if (teamName == "Kiekko-Espoo") return "K-Espoo";
            return "";
        }

        private string TeamNameReverseConverter(string teamName)
        {
            if (teamName == "Ässät") return "Aessaet";
            if (teamName == "Kärpät") return "Kaerpaet";
            if (teamName == "K-Espoo") return "Kiekko-Espoo";
            return "";
        }

        public async Task<int> GetRankingByTeam(string teamName)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var league = Preferences.Get("currentserie", string.Empty);
            if (teamName == "Kärpät" || teamName == "Ässät" || teamName == "K-Espoo")
            {
                teamName = TeamNameReverseConverter(teamName);
            }
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}liigastanding/GetTeamRank?teamName={teamName}&league={league}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var rank = JsonConvert.DeserializeObject<int>(json);
                return rank;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return 0;
            }
        }
    }
}
