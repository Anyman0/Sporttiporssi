using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using Sporttiporssi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sporttiporssi.ViewModels
{
    public class GroupViewModel : INotifyPropertyChanged
    {
        private readonly GroupService _groupService;
        private readonly TeamService _teamService;
        private List<FantasyGroup> _groups { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<FantasyGroup> FantasyGroups
        {
            get => _groups;
            set 
            {
                _groups = value;
                OnPropertyChanged(nameof(FantasyGroups));
            }
        }

        public GroupViewModel(GroupService groupService, TeamService teamService)
        {
            _groupService = groupService;        
            _teamService = teamService;
        }

        public async Task<HockeyDefaultFTP> GetHockeyDefaults()
        {
            return await _teamService.GetHockeyDefaultFTPBySerie();
        }

        public async Task LoadAllFantasyGroupsBySerie() 
        {
            FantasyGroups = await _groupService.AllFantasyGroupsBySerie();
        }

        public async Task<bool> SaveNewGroup(GroupRegisterDto dto)
        {
            bool success = await _groupService.SaveNewGroup(dto);
            return success;
        }

        public async Task<bool> JoinGroup(Guid groupId, Guid teamId, string password)
        {
            bool success = await _groupService.JoinGroup(groupId, teamId, password);
            return success;
        }

        public async Task<bool> LeaveGroup(Guid groupId, Guid teamId)
        {
            //bool success = await _groupService.LeaveGroup(groupId, teamId);
            return true;
        }
    }
}
