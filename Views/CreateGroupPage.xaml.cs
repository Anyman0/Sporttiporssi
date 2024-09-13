using Sporttiporssi.ViewModels;
using Sporttiporssi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Sporttiporssi.Models.DTOs;

namespace Sporttiporssi.Views;

public partial class CreateGroupPage : ContentPage
{
    private GroupViewModel _groupViewModel;
    private readonly IServiceProvider _serviceProvider;
    public CreateGroupPage(GroupViewModel groupViewModel, IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _groupViewModel = groupViewModel;
        BindingContext = _groupViewModel;
        _serviceProvider = serviceProvider;
        LoadDefaultFTPValues();
    }

    private async void LoadDefaultFTPValues()
    {
        // Retrieve default values from your data source
        var defaultFTPValues = await GetDefaultFTPValues();

        TradesPerPhaseMultiplierControl.IsInteger = true;
        TradesPerPhaseMultiplierControl.Value = defaultFTPValues.TradesPerPhase;

        OffencePassMultiplierControl.IsInteger = true;
        OffencePassMultiplierControl.Value = defaultFTPValues.OffencePassFTP;

        OffenceGoalMultiplierControl.IsInteger = true;
        OffenceGoalMultiplierControl.Value = defaultFTPValues.OffenceGoalFTP;

        OffencePenalty10MultiplierControl.IsInteger = true;
        OffencePenalty10MultiplierControl.Value = defaultFTPValues.OffencePenalty10FTP;

        OffencePenalty20MultiplierControl.IsInteger = true;
        OffencePenalty20MultiplierControl.Value = defaultFTPValues.OffencePenalty20FTP;

        OffencePenaltyMultiplierControl.IsInteger = true;
        OffencePenaltyMultiplierControl.Value = defaultFTPValues.OffencePenaltyFTP;

        OffenceShotMultiplierControl.IsInteger = false;
        OffenceShotMultiplierControl.Value = defaultFTPValues.OffenceShotFTP;

        OffencePowerMultiplierControl.IsInteger = false;
        OffencePowerMultiplierControl.Value = defaultFTPValues.OffencePowerFTP;

        DefencePassMultiplierControl.IsInteger = true;
        DefencePassMultiplierControl.Value = defaultFTPValues.DefencePassFTP;

        DefenceGoalMultiplierControl.IsInteger = true;
        DefenceGoalMultiplierControl.Value = defaultFTPValues.DefenceGoalFTP;

        DefencePenaltyMultiplierControl.IsInteger = true;
        DefencePenaltyMultiplierControl.Value = defaultFTPValues.DefencePenaltyFTP;

        DefencePenalty10MultiplierControl.IsInteger = true;
        DefencePenalty10MultiplierControl.Value = defaultFTPValues.DefencePenalty10FTP;

        DefencePenalty20MultiplierControl.IsInteger = true;
        DefencePenalty20MultiplierControl.Value = defaultFTPValues.DefencePenalty20FTP;

        DefenceShotMultiplierControl.IsInteger = false;
        DefenceShotMultiplierControl.Value = defaultFTPValues.DefenceShotFTP;

        DefencePowerMultiplierControl.IsInteger = false;
        DefencePowerMultiplierControl.Value = defaultFTPValues.DefencePowerFTP;

        GoaliePassMultiplierControl.IsInteger = true;
        GoaliePassMultiplierControl.Value = defaultFTPValues.GoaliePassFTP;

        GoalieGoalMultiplierControl.IsInteger = true;
        GoalieGoalMultiplierControl.Value = defaultFTPValues.GoalieGoalFTP;

        GoaliaSaveMultiplierControl.IsInteger = false;
        GoaliaSaveMultiplierControl.Value = defaultFTPValues.GoalieSaveFTP;

        GoalieWinMultiplierControl.IsInteger = true;
        GoalieWinMultiplierControl.Value = defaultFTPValues.GoalieWinFTP;

        FaceOffMultiplierControl.IsInteger = false;
        FaceOffMultiplierControl.Value = defaultFTPValues.FaceOffFTP;

        StartMoneyMultiplierControl.IsInteger = false;
        StartMoneyMultiplierControl.IsBigInteger = true;
        StartMoneyMultiplierControl.Value = defaultFTPValues.StartMoney;
    }

    private async Task<HockeyDefaultFTP> GetDefaultFTPValues()
    {
        return await _groupViewModel.GetHockeyDefaults();
        // Simulate getting default values from a data source
        //return new HockeyDefaultFTP
        //{
        //    TradesPerPhase = 20,
        //    OffencePassFTP = 5,
        //    OffenceGoalFTP = 8,
        //    OffencePenaltyFTP = -1,
        //    OffencePenalty10FTP = -5,
        //    OffencePenalty20FTP = -8,
        //    OffenceShotFTP = 0.5,
        //    OffencePowerFTP = 1.0,
        //    DefencePassFTP = 6,
        //    DefenceGoalFTP = 9,
        //    DefencePenaltyFTP = 1,
        //    DefencePenalty10FTP = -5,
        //    DefencePenalty20FTP = -8,
        //    DefenceShotFTP = 0.5,
        //    DefencePowerFTP = 1.5,
        //    GoaliePassFTP = 10,
        //    GoalieGoalFTP = 25,
        //    GoalieSaveFTP = 0.4,
        //    GoalieWinFTP = 4,
        //    FaceOffFTP = 0.5,
        //    WinningGoalFTP = 3,
        //    StartMoney = 2000000
        //};
    }

    private async void CreateGroup_Clicked(object sender, EventArgs e)
    {       
        var groupName = GroupNameEntry.Text;
        var user = Preferences.Get("currentuser", string.Empty);
        var serie = Guid.Parse("C58DC9C8-074B-4EA6-A0EE-48EA620B0AF6"); // Liiga
       
        var groupPassword = GroupPasswordEntry.Text;
        var createdBy = user;
        var createdDate = DateTime.UtcNow;
        // Create a new FantasyGroup object and set its properties
        var newGroup = new FantasyGroup
        {
            GroupId = Guid.NewGuid(),
            GroupName = groupName,
            Serie = serie,        
            TradesPerPhase = int.Parse(TradesPerPhaseMultiplierControl.Value.ToString()),
            OffencePassFTP = int.Parse(OffencePassMultiplierControl.Value.ToString()),
            OffenceGoalFTP = int.Parse(OffenceGoalMultiplierControl.Value.ToString()),
            OffencePenaltyFTP = int.Parse(OffencePenaltyMultiplierControl.Value.ToString()),
            OffencePenalty10FTP = int.Parse(OffencePenalty10MultiplierControl.Value.ToString()),
            OffencePenalty20FTP = int.Parse(OffencePenalty20MultiplierControl.Value.ToString()),
            OffenceShotFTP = float.Parse(OffenceShotMultiplierControl.Value.ToString()),
            OffencePowerFTP = float.Parse(OffencePowerMultiplierControl.Value.ToString()), // This is +/- statistics
            DefencePassFTP = int.Parse(DefencePassMultiplierControl.Value.ToString()),
            DefenceGoalFTP = int.Parse(DefenceGoalMultiplierControl.Value.ToString()),
            DefencePenaltyFTP = int.Parse(DefencePenaltyMultiplierControl.Value.ToString()),
            DefencePenalty10FTP = int.Parse(DefencePenalty10MultiplierControl.Value.ToString()),
            DefencePenalty20FTP = int.Parse(DefencePenalty20MultiplierControl.Value.ToString()),
            DefenceShotFTP = float.Parse(DefenceShotMultiplierControl.Value.ToString()),
            DefencePowerFTP = float.Parse(DefencePowerMultiplierControl.Value.ToString()), // This is +/- statistics
            GoaliePassFTP = int.Parse(GoaliePassMultiplierControl.Value.ToString()),
            GoalieGoalFTP = int.Parse(GoalieGoalMultiplierControl.Value.ToString()),
            GoalieSaveFTP = float.Parse(GoaliaSaveMultiplierControl.Value.ToString()),
            GoalieWinFTP = int.Parse(GoalieWinMultiplierControl.Value.ToString()),
            GoalieShutoutFTP = int.Parse(GoalieShutoutMultiplierControl.Value.ToString()),
            StartMoney = int.Parse(StartMoneyMultiplierControl.Value.ToString()),
            FaceOffFTP = float.Parse(FaceOffMultiplierControl.Value.ToString()),
            CreatedBy = 0,
            CreatedDate = createdDate,
        };
        var dto = new GroupRegisterDto
        {
            Email = user,
            Password = GroupPasswordEntry.Text,
            FantasyGroup = newGroup,
        };
        // Save the new group to your database
        bool success = await _groupViewModel.SaveNewGroup(dto); // Implement this method to save the group to your database

        if (success)
        {
            // Navigate back or show a success message
            await DisplayAlert("Success", "Group created successfully!", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Failed", "Could not save new group", "OK");
        }
    }
}