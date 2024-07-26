using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using Sporttiporssi.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using Sporttiporssi.Configurations;
using System.Net.Http.Headers;

namespace Sporttiporssi.Services
{
    public class LeagueGamesService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalDatabaseService _databaseService;
        public ObservableCollection<LiigaGameDto> Games { get; set; } = new ObservableCollection<LiigaGameDto>();
        public Dictionary<string, List<string>> GameResults { get; set; } = new Dictionary<string, List<string>>();

        public LeagueGamesService(HttpClient httpClient, LocalDatabaseService localDatabaseService)
        {
            _httpClient = httpClient;
            _databaseService = localDatabaseService;
        }
            
        public async Task<ObservableCollection<LiigaGameDto>> LoadGamesAsync(DateTime date)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var gameResponse = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}Games?date={date}");              
                if(gameResponse.IsSuccessStatusCode)
                {
                    var gamesByDate = await gameResponse.Content.ReadFromJsonAsync<ObservableCollection<LiigaGameDto>>();
                    if(GameResults.Count == 0)
                    {
                        GameResults = await GetLastGamesForAllTeams();
                    }
                    foreach(var game in gamesByDate)
                    {
                        game.HomeTeamLogo = TeamLogoHelper.GetTeamLogo(game.HomeTeam.TeamName);
                        game.AwayTeamLogo = TeamLogoHelper.GetTeamLogo(game.AwayTeam.TeamName);
                        game.Start = game.Start.ToLocalTime();
                        game.HomeTeam.Ranking = _databaseService.GetAllStandingsAsync().Result.Where(s => s.TeamName.ToLower() == game.HomeTeam.TeamName.ToLower()).Select(x => x.Ranking).FirstOrDefault();
                        game.AwayTeam.Ranking = _databaseService.GetAllStandingsAsync().Result.Where(s => s.TeamName.ToLower() == game.AwayTeam.TeamName.ToLower()).Select(x => x.Ranking).FirstOrDefault();
                        if(GameResults.TryGetValue(game.HomeTeam.TeamName, out var homeTeamResults))
                        {
                            game.LastHomeGames = string.Join(" ", homeTeamResults);
                        }
                        else
                        {
                            game.LastHomeGames = "- - -";
                        }
                        if(GameResults.TryGetValue(game.AwayTeam.TeamName, out var awayTeamResults))
                        {
                            game.LastAwayGames = string.Join(" ", awayTeamResults);
                        }
                        else
                        {
                            game.LastAwayGames = "- - -";
                        }
                    }
                    Games = gamesByDate;
                }               
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error fetching games: {ex.Message}");
            }
            return Games;           
        }

        public async Task<LiigaGameStatsDto> GetGameStatsAsync(int gameId, int season)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var gameStatsResponse = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}Games/{gameId}/{season}");
            if(gameStatsResponse.IsSuccessStatusCode)
            {
                var json = await gameStatsResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LiigaGameStatsDto>(json);
            }
            else
            {
                throw new Exception("Error fetching gamestats");
            }
        }

        public async Task<Dictionary<string, List<string>>> GetLastGamesForAllTeams()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var gameResultResponse = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}Games/LastGamesForAll");
            if(!gameResultResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching gameresults from API");
            }
            var json = await gameResultResponse.Content.ReadAsStringAsync();
            var gamesDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            return gamesDictionary;
        }
      
    }
}
