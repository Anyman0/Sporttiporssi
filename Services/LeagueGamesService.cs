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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sporttiporssi.Services
{
    public class LeagueGamesService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalDatabaseService _databaseService;
        private readonly LeagueStandingsService _leagueStandingsService;
        public ObservableCollection<LiigaGameDto> Games { get; set; } = new ObservableCollection<LiigaGameDto>();
        public Dictionary<string, List<string>> GameResults { get; set; } = new Dictionary<string, List<string>>();

        public LeagueGamesService(HttpClient httpClient, LocalDatabaseService localDatabaseService, LeagueStandingsService leagueStandingsService)
        {
            _httpClient = httpClient;
            //var unsafeHttpClient = new UnsafeHttpClientHandler();
            //_httpClient = new HttpClient(unsafeHttpClient);
            _databaseService = localDatabaseService;
            _leagueStandingsService = leagueStandingsService;
        }
      
        public async Task<ObservableCollection<Game>> LoadGamesByDate(DateTime date)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var league = Preferences.Get("currentserie", string.Empty);
            var season = date.Year;
            var gamesByDate = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}Games?date={date}&serie={league}");
            if (gamesByDate.IsSuccessStatusCode)
            {
                var json = await gamesByDate.Content.ReadAsStringAsync();                
                var gameObject = JsonConvert.DeserializeObject<ObservableCollection<Game>>(json);
                if (gameObject != null && gameObject.Count > 0)
                {
                    foreach (var game in gameObject)
                    {
                        game.HomeTeamLogo = TeamLogoHelper.GetTeamLogo(game.HomeTeamName);
                        game.AwayTeamLogo = TeamLogoHelper.GetTeamLogo(game.AwayTeamName);
                        game.HomeTeamRank = await _leagueStandingsService.GetRankingByTeam(game.HomeTeamName);
                        game.AwayTeamRank = await _leagueStandingsService.GetRankingByTeam(game.AwayTeamName);                        
                    }
                }
                else
                {
                    gameObject = new ObservableCollection<Game>();
                }
                return gameObject;
            }
            else
            {
                throw new Exception("Error fetching gamestats");
            }
        }

        public async Task<LiigaGameStatsDto> GetGameRosterAsync(string hometeam, string awayteam, DateTime gameDate)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var gameStatsResponse = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}Games/GetRoster?hometeam={hometeam}&awayteam={awayteam}&gameDate={gameDate.ToString()}");
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
