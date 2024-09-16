using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Services
{
    public class LocalDatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public LocalDatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<LeagueStanding>().Wait();
            //_database.CreateTableAsync<Game>().Wait();
            //_database.CreateTableAsync<Player>().Wait();
        }

        // **** FETCHES **** //
        //public Task<List<LeagueStanding>> GetAllStandingsAsync()
        //{
        //    return _database.Table<LeagueStanding>().ToListAsync();
        //}     
        public Task<List<Game>> GetAllGamesAsync()
        {
            return _database.Table<Game>().ToListAsync();
        }


        public async Task<ObservableCollection<Game>> GetGamesByDateAsync(DateTime date)
        {
            var nextDay = date.AddDays(1);
            var gamesList = await _database.Table<Game>().Where(g => g.Start >= date && g.Start < nextDay).ToListAsync();
            var gamesCollection = new ObservableCollection<Game>(gamesList);
            return gamesCollection;
        } 

        public Task<List<Player>> GetAllPlayersAsync()
        {
            return _database.Table<Player>().ToListAsync();
        }

        public async Task<ObservableCollection<Player>> GetPlayersByTeamId(int teamId)
        {
            var playerList = await _database.Table<Player>().Where(p => p.TeamId == teamId).ToListAsync();
            var playerCollection = new ObservableCollection<Player>(playerList);
            return playerCollection;
        }

        public async Task<Player> GetPlayerById(int playerId)
        {
            return await _database.Table<Player>().Where(p => p.PlayerId == playerId).FirstOrDefaultAsync();
        }

        public async Task<Game> GetGameAsync(int id)
        {
            return await _database.Table<Game>().Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        //public async Task<LeagueStanding> GetStandingAsync(string id)
        //{
        //    return await _database.Table<LeagueStanding>().Where(s => s.TeamId == id).FirstOrDefaultAsync();
        //}
      
        // **** INSERTS **** //
        public Task<int> SaveSeasonAsync(LeagueStanding season)
        {
            return _database.InsertOrReplaceAsync(season);
        }

        public Task<int> SaveGameAsync(Game game) 
        {
            Debug.WriteLine("Saving game with ID: " + game.Id);
            return _database.InsertAsync(game);
        }

        public Task<int> SavePlayerAsync(Player player)
        {
            Debug.WriteLine("Saving player with ID: " + player.PlayerId);
            return _database.InsertAsync(player);
        }  

        // **** UPDATES **** //
        public async Task<int> UpdateStandingAsync(LeagueStanding season)
        {
            return await _database.UpdateAsync(season);
        }

        public async Task<int> UpdateGameAsync(Game game)
        {
            return await _database.UpdateAsync(game);
        }

        public async Task<int> UpdatePlayerAsync(Player player)
        {
            return await _database.UpdateAsync(player);
        }

        // **** DELETES **** //
        public Task<int> DeleteStandingAsync(LeagueStanding season)
        {
            return _database.DeleteAsync(season);
        }

        public Task<int> DeleteGameAsync(Game game)
        {
            return _database.DeleteAsync(game);
        }

        public Task<int> DeletePlayerAsync(Player player)
        {
            return _database.DeleteAsync(player);
        }

        public Task<int> ClearStandingsAsync()
        {
            return _database.DeleteAllAsync<LeagueStanding>();
        }

        public Task<int> ClearAllGamesAsync()
        {
            return _database.DeleteAllAsync<Game>();
        }

        public Task<int> ClearAllPlayersAsync()
        {
            return _database.DeleteAllAsync<Player>();
        }
    }
}
