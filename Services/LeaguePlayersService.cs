using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Sporttiporssi.Configurations;
using System.Net.Http.Headers;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sporttiporssi.Services
{
    
    public class LeaguePlayersService
    {
        private readonly LocalDatabaseService _databaseService;
        private readonly HttpClient _httpClient;

        private readonly string Liiga_baseApiUrl = "https://www.liiga.fi/api/v2/";
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        
        public LeaguePlayersService(LocalDatabaseService databaseService, HttpClient httpClient)
        {
            _databaseService = databaseService;
            _httpClient = httpClient;
            //var unsafeHttpClient = new UnsafeHttpClientHandler();
            //_httpClient = new HttpClient(unsafeHttpClient);
        }

        public async Task<ObservableCollection<Player>> GetPlayersAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}player");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return new ObservableCollection<Player>();
            }            
        }  
        
        public async Task<ObservableCollection<Player>> GetPlayersByRole(string[] roles)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var rolesParam = string.Join("&roles=", roles);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}player/GetPlayersByRole?roles={rolesParam}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return new ObservableCollection<Player>();
            }
        }
        public async Task<ObservableCollection<Player>> GetPlayersByFantasyTeamName(string teamName)
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();           
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}player/PlayersByFantasyTeam?teamName={teamName}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return new ObservableCollection<Player>();
            }
        }

        public async Task<bool> AddPlayersToTeam(int playerId)
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            string chosenTeam = await SecureStorage.GetAsync("chosen_team");
            Guid fantasyTeamId = Guid.Parse(chosenTeam);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {               
                var response = await _httpClient.PostAsync($"{ApiConfig.ApiBaseAddress}Player/AddPlayerToTeam?playerId={playerId}&fantasyTeamId={fantasyTeamId}", null);
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

        public async Task<ObservableCollection<Player>> GetPlayersByTeamId(int teamId)
        {
            var teamPlayerList = await _databaseService.GetPlayersByTeamId(teamId);
            return teamPlayerList;
        }

        private async Task LoadPlayersAsync()
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            try
            {
                var response = await _httpClient.GetAsync($"{ApiConfig.ApiBaseAddress}player");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                Players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                Players =  new ObservableCollection<Player>();
            }

            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        var playerResponse = await client.GetStringAsync(apiUrl);
            //        var allPlayersDto = JsonConvert.DeserializeObject<List<Player>>(playerResponse);                    
            //        foreach (var playerDto in allPlayersDto)
            //        {
            //            var player = new Player
            //            {
            //                PlayerId = playerDto.PlayerId,
            //                TeamId = playerDto.TeamId,
            //                TeamName = playerDto.TeamName,
            //                TeamShortName = playerDto.TeamShortName,
            //                Role = playerDto.Role,
            //                FirstName = playerDto.FirstName,
            //                LastName = playerDto.LastName,
            //                Nationality = playerDto.Nationality,
            //                Tournament = playerDto.Tournament,
            //                PictureUrl = playerDto.PictureUrl,
            //                PreviousTeamsForTournament = playerDto.PreviousTeamsForTournament,
            //                Injured = playerDto.Injured,
            //                Jersey = playerDto.Jersey,
            //                LastSeason = playerDto.LastSeason,
            //                Goalkeeper = playerDto.Goalkeeper,
            //                Games = playerDto.Games,
            //                PlayedGames = playerDto.PlayedGames,
            //                Rookie = playerDto.Rookie,
            //                Suspended = playerDto.Suspended,
            //                Removed = playerDto.Removed,
            //                TimeOnIce = playerDto.TimeOnIce,
            //                Current = playerDto.Current,
            //                Goals = playerDto.Goals,
            //                Assists = playerDto.Assists,
            //                Points = playerDto.Points,
            //                Plus = playerDto.Plus,
            //                Minus = playerDto.Minus,
            //                PlusMinus = playerDto.PlusMinus,
            //                PenaltyMinutes = playerDto.PenaltyMinutes,
            //                PowerplayGoals = playerDto.PowerplayGoals,
            //                PenaltykillGoals = playerDto.PenaltykillGoals,
            //                WinningGoals = playerDto.WinningGoals,
            //                Shots = playerDto.Shots,
            //                ShotsIntoGoal = playerDto.ShotsIntoGoal,
            //                FaceoffsWon = playerDto.FaceoffsWon,
            //                FaceoffsLost = playerDto.FaceoffsLost,
            //                ExpectedGoals = playerDto.ExpectedGoals,
            //                TimeOnIceAvg = playerDto.TimeOnIceAvg,
            //                FaceoffWonPercentage = playerDto.FaceoffWonPercentage,
            //                ShotPercentage = playerDto.ShotPercentage,
            //                FaceoffsTotal = playerDto.FaceoffsTotal,
            //                LastUpdated = DateTime.UtcNow,
            //                Price = playerDto.Price,
            //                FTP = playerDto.FTP,
            //            };
            //            Players.Add(player);

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Couldn't get players. Error: " + ex.Message);
            //    }
            //}
        }
    }
}
