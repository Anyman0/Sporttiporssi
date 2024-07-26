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
        }

        public async Task<List<Player>> GetPlayersAsync()
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
                return JsonConvert.DeserializeObject<List<Player>>(json);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error fetching players: {ex.Message}");
                return new List<Player>();
            }            
        }       
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            var localPlayerList = await _databaseService.GetAllPlayersAsync(); 
            // Update all players every 2 days
            var needToUpdate = localPlayerList.Any(s => s.LastUpdated < DateTime.UtcNow.AddDays(-2));
            if (localPlayerList.Count == 0 || needToUpdate)
            {
                await LoadPlayersAsync();
                if (Players != null)
                {
                    foreach (var player in Players)
                    {
                        var existingPlayer = await _databaseService.GetPlayerById(player.PlayerId);
                        if (existingPlayer != null)
                        {
                            player.LastUpdated = DateTime.UtcNow;
                            await _databaseService.UpdatePlayerAsync(player);                           
                        }
                        else
                        {
                            await _databaseService.SavePlayerAsync(player);
                        }                      
                    }

                    return Players.ToList();
                }
            }
            return localPlayerList;
        }

        public async Task<ObservableCollection<Player>> GetPlayersByTeamId(int teamId)
        {
            var teamPlayerList = await _databaseService.GetPlayersByTeamId(teamId);
            return teamPlayerList;
        }

        private async Task LoadPlayersAsync()
        {
            string apiUrl = Liiga_baseApiUrl + "players/stats/summed/2025/2025/runkosarja/true?dataType=basicStats";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var playerResponse = await client.GetStringAsync(apiUrl);
                    var allPlayersDto = JsonConvert.DeserializeObject<List<Player>>(playerResponse);                    
                    foreach (var playerDto in allPlayersDto)
                    {
                        var player = new Player
                        {
                            PlayerId = playerDto.PlayerId,
                            TeamId = playerDto.TeamId,
                            TeamName = playerDto.TeamName,
                            TeamShortName = playerDto.TeamShortName,
                            Role = playerDto.Role,
                            FirstName = playerDto.FirstName,
                            LastName = playerDto.LastName,
                            Nationality = playerDto.Nationality,
                            Tournament = playerDto.Tournament,
                            PictureUrl = playerDto.PictureUrl,
                            PreviousTeamsForTournament = playerDto.PreviousTeamsForTournament,
                            Injured = playerDto.Injured,
                            Jersey = playerDto.Jersey,
                            LastSeason = playerDto.LastSeason,
                            Goalkeeper = playerDto.Goalkeeper,
                            Games = playerDto.Games,
                            PlayedGames = playerDto.PlayedGames,
                            Rookie = playerDto.Rookie,
                            Suspended = playerDto.Suspended,
                            Removed = playerDto.Removed,
                            TimeOnIce = playerDto.TimeOnIce,
                            Current = playerDto.Current,
                            Goals = playerDto.Goals,
                            Assists = playerDto.Assists,
                            Points = playerDto.Points,
                            Plus = playerDto.Plus,
                            Minus = playerDto.Minus,
                            PlusMinus = playerDto.PlusMinus,
                            PenaltyMinutes = playerDto.PenaltyMinutes,
                            PowerplayGoals = playerDto.PowerplayGoals,
                            PenaltykillGoals = playerDto.PenaltykillGoals,
                            WinningGoals = playerDto.WinningGoals,
                            Shots = playerDto.Shots,
                            ShotsIntoGoal = playerDto.ShotsIntoGoal,
                            FaceoffsWon = playerDto.FaceoffsWon,
                            FaceoffsLost = playerDto.FaceoffsLost,
                            ExpectedGoals = playerDto.ExpectedGoals,
                            TimeOnIceAvg = playerDto.TimeOnIceAvg,
                            FaceoffWonPercentage = playerDto.FaceoffWonPercentage,
                            ShotPercentage = playerDto.ShotPercentage,
                            FaceoffsTotal = playerDto.FaceoffsTotal,
                            LastUpdated = DateTime.UtcNow
                        };
                        Players.Add(player);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't get players. Error: " + ex.Message);
                }
            }
        }
    }
}
